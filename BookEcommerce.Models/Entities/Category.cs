using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Category
    {
        public Category()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategory { get; set; }
      
        public string? VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }

        //public string? ImageId { get; set; }
        public virtual Image? Image { get; set; }

        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }

    }
}
