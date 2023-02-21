using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class ProductPrice
    {
        public ProductPrice()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(255)]
        public string? ProductPriceId { get; set; }
        public double ProductVariantDefaultPrice { get; set; }
        public double PruductVariantSalePrice { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public virtual ProductVariant? ProductVariant { get; set; }
    }
}
