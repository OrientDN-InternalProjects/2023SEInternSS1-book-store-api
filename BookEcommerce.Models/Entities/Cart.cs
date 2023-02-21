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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(255)]
        public string? CartId { get; set; }
        [MaxLength(255)]
        public string? ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string? CustomerId { get; set; }
        [NotMapped]
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
        [NotMapped]
        public Customer? Customer { get; set; }
    }
}
