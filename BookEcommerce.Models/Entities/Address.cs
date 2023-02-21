using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Address
    {
        public Address()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? AddressId { get; set; }
        public string? StreetAddress { get; set; }
        public string? Country { get; set; }
        public string? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
