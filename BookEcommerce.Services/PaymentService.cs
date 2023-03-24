using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Options;
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
        private readonly IOrderService orderService;
        private string? paymentId;
        public PaymentService(IUnitOfWork unitOfWork, 
                              IPaymentRepository paymentRepository,
                              IConfiguration configuration,
                              IHttpContextAccessor httpContextAccessor,
                              IOrderRepository orderRepository,
                              ILogger<PaymentService> logger, 
                              IPaypalContextService paypalContextService, 
                              IOrderService orderService) : base(unitOfWork)
        {
            this.paymentRepository = paymentRepository;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.orderRepository = orderRepository;
            this.logger = logger;
            this.paypalContextService = paypalContextService;
            this.orderService = orderService;
        }

        public async Task<PaymentResponse> CreatePaymentWithPaypal(Guid? orderId, string paymentId, string? payerId, string cancel = null)
        {
            try
            {
                var context = GetPaypalContext();
                var ApiContext = ImplementApiContext(context);
                if (payerId == null)
                {
                    var order = await GetOrder(orderId);
                    var baseUrl = await GenerateBaseUrl(order.OrderId);
                    var createdPayment = CreatePaymentBaseOnBaseUrl(ApiContext, baseUrl, order);
                    var Links = GetPaypalLink(createdPayment);
                    var paypalRedirectUrl = CreateRedirectUrl(Links);
                    CreatePaymentSession(createdPayment);
                    this.paymentId = GetPaymentIdFromSession("payment");
                    return new PaymentResponse
                    {
                        IsSuccess = true,
                        Message = "Prepare to pay",
                        RedirectUrl = paypalRedirectUrl,
                        PayerId = payerId,
                        PaymentId = this.paymentId
                    };
                }
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "failed to open payment phase",
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "failed to open payment phase"
                };
            }
        }

        public async Task<PaymentResponse> ExecutePayment(Guid orderId, string payerId, string paymentId)
        {
            try
            {
                var context = GetPaypalContext();
                var apiContext = ImplementApiContext(context);
                this.paymentId = GetPaymentIdFromSession("payment");
                logger.LogInformation(paymentId);
                var executedPayment = ExecutePayment(apiContext, payerId, paymentId);
                if (executedPayment.state.ToLower() != "approved")
                {
                    await this.orderService.ChangeStatusOrder(new Models.DTOs.Request.StatusRequest { StatusOrder = "Cancel" }, orderId);
                    return new PaymentResponse
                    {
                        IsSuccess = false,
                        Message = "Somethings went wrong with payment functionality, retry?",
                    };
                }
                await this.paymentRepository.SaveToPaymentHistory(executedPayment.state.ToUpper(), orderId);
                await this.orderService.ChangeStatusOrder(new Models.DTOs.Request.StatusRequest {StatusOrder = "Accepted"}, orderId);
                await this._unitOfWork.CommitTransaction();
                return new PaymentResponse
                {
                    IsSuccess = true,
                    Message = "Payment success",
                    Transaction = executedPayment.transactions[0]
                };
            }
            catch(Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "something wrong on execution phase, please retry!",
                };
            }
        }

        public PaypalContext GetPaypalContext()
        {
            var context = this.paypalContextService.GetPaypalContext();
            return context;
        }

        public APIContext ImplementApiContext(PaypalContext context)
        {
            var apiContext = PaypalConfiguration.GetApiContext(
                context.ClientId!,
                context.ClientSecret!
            );
            return apiContext;
        }

        public async Task<string> GenerateBaseUrl(Guid? orderId)
        {
            var order = await GetOrder(orderId);
            string baseURL = $"https://localhost:7018/execute?orderId={order.OrderId}&";
            return baseURL;
        }

        public async Task<BookEcommerce.Models.Entities.Order> GetOrder(Guid? orderId)
        {
            var currentOrder = await this.orderRepository.GetOrderByOrderId(orderId);
            return currentOrder;
        }

        public Payment CreatePaymentBaseOnBaseUrl(APIContext apiContext, string baseUrl, BookEcommerce.Models.Entities.Order order)
        {
            var guid = Guid.NewGuid().ToString();
            var createdPayment = this.paymentRepository.CreatePayment(apiContext, baseUrl + "guid=" + guid, order);
            return createdPayment;
        }

        public List<Links>.Enumerator GetPaypalLink(Payment paymentCreated)
        {
            var links = paymentCreated.links.GetEnumerator();
            return links;
        }

        public string CreateRedirectUrl(List<Links>.Enumerator links)
        {
            var paypalRedirectUrl = string.Empty;
            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                {
                    paypalRedirectUrl = lnk.href;
                    return paypalRedirectUrl;
                }
            }
            return "Invalid link";
        }

        public void CreatePaymentSession(Payment paymentCreated)
        {
            this.httpContextAccessor.HttpContext!.Session.SetString("payment", paymentCreated.id);
        }

        public string GetPaymentIdFromSession(string key)
        {
            var paymentId = this.httpContextAccessor.HttpContext!.Session.GetString("payment");
            return paymentId;
        }

        public Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var executedPayment = this.paymentRepository.ExecutePayment(apiContext, payerId, paymentId);
            return executedPayment;
        }
    }
}
