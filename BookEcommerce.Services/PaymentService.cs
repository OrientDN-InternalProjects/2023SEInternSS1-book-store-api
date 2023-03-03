using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Paypal;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.Api;
using System.Text;

namespace BookEcommerce.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<PaymentService> logger;
        private readonly IPaypalContextService paypalContextService;
        private string? paymentId;
        public PaymentService(IUnitOfWork unitOfWork, IPaymentRepository paymentRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository, ILogger<PaymentService> logger, IPaypalContextService paypalContextService) : base(unitOfWork)
        {
            this.paymentRepository = paymentRepository;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.orderRepository = orderRepository;
            this.logger = logger;
            this.paypalContextService = paypalContextService;
        }

        public string CreatePayerId()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<PaymentResponse> CreatePaymentWithPaypal(Guid orderId, string paymentId, string? payerId, string Cancel = null)
        {
            try
            {
                var Request = this.httpContextAccessor.HttpContext.Request;
                var context = this.paypalContextService.GetPaypalContext();
                APIContext ApiContext = PaypalConfiguration.GetApiContext(
                  context.ClientId!,
                  context.ClientSecret!
                );
                if (payerId == null)
                {
                    var guid = Guid.NewGuid().ToString();
                    var currentOrder = await this.orderRepository.GetOrderByOrderId(orderId);
                    string BaseURL = $"https://localhost:7018/execute?orderId={orderId}&";
                    var createdPayment = this.paymentRepository.CreatePayment(ApiContext, BaseURL + "guid=" + guid, currentOrder);
                    var Links = createdPayment.links.GetEnumerator();
                    string PaypalRedirectUrl = String.Empty;
                    while (Links.MoveNext())
                    {
                        Links lnk = Links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            PaypalRedirectUrl = lnk.href;
                        }
                    }
                    this.httpContextAccessor.HttpContext!.Session.SetString("payment", createdPayment.id);
                    this.paymentId = this.httpContextAccessor.HttpContext!.Session.GetString("payment");
                    await this._unitOfWork.CommitTransaction();
                    if (paymentId == null)
                    {
                        return new PaymentResponse
                        {
                            IsSuccess = true,
                            Message = "Prepare to pay",
                            RedirectUrl = PaypalRedirectUrl,
                            PayerId = payerId,
                            PaymentId = this.paymentId
                        };
                    }
                    return new PaymentResponse
                    {
                        IsSuccess = false,
                        Message = "failed to serve url",
                        RedirectUrl = PaypalRedirectUrl,
                        PayerId = payerId,
                        PaymentId = this.paymentId
                    };
                }
                else
                {
                    return new PaymentResponse
                    {
                        IsSuccess = false
                    };
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<PaymentResponse> ExecutePayment(Guid orderId, string payerId, string paymentId)
        {
            try
            {
                var context = this.paypalContextService.GetPaypalContext();
                APIContext ApiContext = PaypalConfiguration.GetApiContext(
                  context.ClientId!,
                  context.ClientSecret!
                );
                this.httpContextAccessor.HttpContext!.Session.SetString("payment", paymentId);
                this.paymentId = this.httpContextAccessor.HttpContext!.Session.GetString("payment");
                logger.LogInformation(paymentId);
                var executedPayment = this.paymentRepository.ExecutePayment(ApiContext, payerId, paymentId);
                if (executedPayment.state.ToLower() != "approved")
                {
                    return new PaymentResponse
                    {
                        IsSuccess = false,
                        Message = "Somethings went wrong with payment functionality, retry?",
                    };
                }
                return new PaymentResponse
                {
                    IsSuccess = true,
                    Message = "Payment success",
                    Transaction = executedPayment.transactions[0]
                };
            }
            catch(Exception e)
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = e.Message,
                };
            }
        }
    }
}
