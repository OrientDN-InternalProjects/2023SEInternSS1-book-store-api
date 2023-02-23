using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Cart
    {
        public Cart()
        {
            ProductVariants = new HashSet<ProductVariant>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? CartId { get; set; }
        public string? ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }

        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
