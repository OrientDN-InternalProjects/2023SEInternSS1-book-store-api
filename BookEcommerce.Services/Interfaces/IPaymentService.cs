using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentResponse> CreatePaymentWithPaypal(Guid orderId, string? PayerId, string Cancel = null);
    }
}
