using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class ProductRequest
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? VendorId { get; set; }
        public string? CategoryId { get; set; }
        public List<string>? Paths { get; set; }
        public List<ProductVariantRequest>? ProductVariants { get; set; }
    }
}
