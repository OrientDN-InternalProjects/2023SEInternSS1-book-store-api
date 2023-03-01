using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class ProductVariantRequest
    {
        public string? ProductVariantName { get; set; }
        public int Quantity { get; set; }
        public double ProductDefautPrice { get; set; }
        public double ProductSalePrice { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
