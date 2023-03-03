using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
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
        public Task<PaymentResponse> CreatePaymentWithPaypal(Guid orderId, string paymentId, string? payerId, string Cancel = null);
        public Task<PaymentResponse> ExecutePayment(Guid orderId, string payerId, string paymentId);
    }
}
