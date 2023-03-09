using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class PhoneNumber
    {
        public PhoneNumber()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? PhoneNumberId { get; set; }
        public string? PhoneNum { get; set; }
        public virtual Guid? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
