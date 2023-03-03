using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Options
{
    public class PaypalContext
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public PaypalContext(IConfiguration configuration)
        {
            this.ClientId = configuration["Paypal:ClientId"];
            this.ClientSecret = configuration["Paypal:ClientSecret"];
        }
    }
}
