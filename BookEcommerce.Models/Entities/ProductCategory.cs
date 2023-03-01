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
        public Guid? ProductCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
