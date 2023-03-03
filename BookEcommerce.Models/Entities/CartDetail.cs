using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class CartDetail
    {
        public CartDetail()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CartDetailId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public Guid? CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
