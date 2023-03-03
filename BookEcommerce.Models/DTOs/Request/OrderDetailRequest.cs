using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class OrderDetailRequest
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
