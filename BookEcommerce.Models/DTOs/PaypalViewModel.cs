using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class PaypalViewModel
    {
        public string? PayerId { get; set; }
        public string? PaymentId { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
