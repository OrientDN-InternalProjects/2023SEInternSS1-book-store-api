using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? PaymentId { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentStatus { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
