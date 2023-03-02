using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Paypal;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Services.Base;
using Microsoft.Extensions.Logging;

namespace BookEcommerce.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<PaymentService> logger;
        public PaymentService(IUnitOfWork unitOfWork, IPaymentRepository paymentRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository, ILogger<PaymentService> logger) : base(unitOfWork)
        {
            this.paymentRepository = paymentRepository;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.orderRepository = orderRepository;
            this.logger = logger;
        }

        public async Task<PaymentResponse> CreatePaymentWithPaypal(Guid orderId, string? PayerId, string Cancel = null)
        {
            try
            {
                var Request = this.httpContextAccessor.HttpContext.Request;
                string clientId = configuration.GetValue<string>("Paypal:ClientId");
                string clientSecret = configuration.GetValue<string>("Paypal:ClientSecret");
                APIContext ApiContext = PaypalConfiguration.GetApiContext(
                  clientId,
                  clientSecret
                );
                if (string.IsNullOrEmpty(PayerId))
                {
                    var guid = Guid.NewGuid().ToString();
                    var CurrentOrder = await this.orderRepository.GetOrderByOrderId(orderId);
                    string BaseURL = $"https://localhost:7018/payment-with-paypal?orderId={orderId}&";
                    var CreatedPayment = this.paymentRepository.CreatePayment(ApiContext, BaseURL + "guid=" + guid, CurrentOrder);
                    var Links = CreatedPayment.links.GetEnumerator();
                    string PaypalRedirectUrl = String.Empty;
                    while (Links.MoveNext())
                    {
                        Links lnk = Links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            PaypalRedirectUrl = lnk.href;
                        }
                    }
                    this.httpContextAccessor.HttpContext!.Session.SetString("payment", CreatedPayment.id);
                    await this._unitOfWork.CommitTransaction();
                    return new PaymentResponse
                    {
                        IsSuccess = true,
                        Message = "Prepare to pay",
                        RedirectUrl = PaypalRedirectUrl,
                    };
                }       
                    var PaymentId = httpContextAccessor.HttpContext!.Session.GetString("payment");
                    var executedPayment = this.paymentRepository.ExecutePayment(ApiContext, PayerId, PaymentId);
                    logger.LogInformation(executedPayment.links.ToString());
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
                        Message = executedPayment.transactions[0].ToString()
                    };
                
            }
            catch(Exception e)
            {
                logger.LogError(e.ToString());
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}
