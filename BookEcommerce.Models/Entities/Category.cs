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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(255)]
        public string? CategoryId { get; set; }
        [MaxLength(255)]
        public string? CategoryName { get; set; }
        [MaxLength(255)]
        public string? SubCategory { get; set; }
        [ForeignKey("FK_Category_Vendor")]
        [MaxLength(255)]
        public string? VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }
        public string? ImageId { get; set; }
        [NotMapped]
        public virtual Image? Images { get; set; }

        [NotMapped]
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }

    }
}
