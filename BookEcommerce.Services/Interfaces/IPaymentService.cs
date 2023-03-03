using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Options;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IPaymentService
    {
        public PaypalContext GetPaypalContext();
        public APIContext ImplementApiContext(PaypalContext context);
        public Task<BookEcommerce.Models.Entities.Order> GetOrder(Guid? orderId);
        public Task<string> GenerateBaseUrl(Guid? orderId);
        public Payment CreatePaymentBaseOnBaseUrl(APIContext apiContext, string baseUrl, BookEcommerce.Models.Entities.Order order);
        public string CreateRedirectUrl(List<Links>.Enumerator links);
        public void CreatePaymentSession(Payment paymentCreated);
        public string GetPaymentIdFromSession(string key);
        public Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId);
        public Task<PaymentResponse> CreatePaymentWithPaypal(Guid? orderId, string paymentId, string? payerId, string cancel = null);
        public PaymentResponse ExecutePayment(Guid orderId, string payerId, string paymentId);
    }
}
