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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(255)]
        public string? PaymentId { get; set; }
        [MaxLength(255)]
        public string? PaymentType { get; set; }
        [MaxLength(255)]
        public string? PaymentStatus { get; set; }
        [NotMapped]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
