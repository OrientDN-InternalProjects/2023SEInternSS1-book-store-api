using BookEcommerce.Models.Options;
using BookEcommerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class PaypalContextService : IPaypalContextService
    {
        public PaypalContext PaypalContext { get; set; }
        public PaypalContextService(PaypalContext paypalContext)
        {
            this.PaypalContext = paypalContext;
        }

        public PaypalContext GetPaypalContext()
        {
            return PaypalContext;
        }
    }
}
