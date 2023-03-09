using BookEcommerce.Models.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IPaypalContextService
    {
        public PaypalContext GetPaypalContext();
    }
}
