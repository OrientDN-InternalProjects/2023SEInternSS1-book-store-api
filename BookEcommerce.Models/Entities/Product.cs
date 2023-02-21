using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Product
    {
        public Product()
        {
            Images = new HashSet<Image>();
            ProductCategories = new HashSet<ProductCategory>();
            ProductVariants = new HashSet<ProductVariant>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        [ForeignKey("FK_Product_Vendor")]
        public string? VendorId { get; set; }
        public string? ProductDecription { get; set; }
        [ForeignKey("FK_Product_ProductVariant")]
        public string? ProductVariantId { get; set; }
        public virtual Vendor? Vendor { get; set; }
        [NotMapped]
        public virtual ICollection<Image>? Images { get; set; }
        [NotMapped]
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
        [NotMapped]
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
}
}
