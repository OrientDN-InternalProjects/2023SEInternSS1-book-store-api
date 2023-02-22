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
            //Images = new HashSet<Image>();
            //ProductCategories = new HashSet<ProductCategory>();
            //ProductVariants = new HashSet<ProductVariant>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDecription { get; set; }
        public string? VendorId { get; set; }
        public  Vendor? Vendor { get; set; }
        public  ICollection<Image>? Images { get; set; }
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
}
}
 