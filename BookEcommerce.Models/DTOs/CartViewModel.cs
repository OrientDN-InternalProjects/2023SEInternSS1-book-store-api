using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class CartViewModel : ResponseBase
    {
        public Guid? ProductVariantId { get; set; }
        public string? ProductVariantName { get; set; }
        public int Quantity { get; set; }
        public double? Total { get; set; }
    }
}
