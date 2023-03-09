using BookEcommerce.Models.DTOs.Response.Base;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Response
{
    public class PaymentResponse : ResponseBase
    {
        public string? RedirectUrl { get; set; }
        public string? PayerId { get; set; }
        public string? PaymentId { get; set; }
        public Transaction? Transaction { get; set; }
    }
}
