using BookEcommerce.Models.DTOs;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        public Payment CreatePayment(APIContext apiContext, string redirectURL, BookEcommerce.Models.Entities.Order order);
        public Payment ExecutePayment(APIContext ApiContext, string PayerId, string PaymentId);
        public Task<bool> SaveToPaymentHistory(string status, Guid orderId);
    }
}
