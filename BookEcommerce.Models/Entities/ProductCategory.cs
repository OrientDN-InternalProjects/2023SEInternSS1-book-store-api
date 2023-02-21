using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(255)]
        public string? IdProductCategory { get; set; }
        [ForeignKey("FK_CategoryProduct_Category")]
        public string? CategoryId { get; set; }
        [ForeignKey("FK_CategoryProduct_Product")]
        public string? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Category? Category { get; set; }
    }
}
