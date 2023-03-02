using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookEcommerce.Models.Entities;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        public PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectURL, BookEcommerce.Models.Entities.Order order);
        public PayPal.Api.Payment ExecutePayment(APIContext ApiContext, string PayerId, string PaymentId);
    }
}
