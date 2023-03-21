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
            CartDetails = new HashSet<CartDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid? CartId { get; set; }
        public virtual ICollection<CartDetail>? CartDetails { get; set; }
        public virtual Guid? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
