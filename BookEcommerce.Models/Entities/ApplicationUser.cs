using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookEcommerce.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid? VendorId { get; set; }
        public Vendor? Vendor { get; set; }

        public Guid? AdminId { get; set; }
        public Admin? Admin { get; set; }
    }
}
