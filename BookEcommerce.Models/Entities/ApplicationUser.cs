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
        public virtual Customer? Customer { get; set; }
        public virtual Vendor? Vendor { get; set; }
        public virtual Admin? Admin { get; set; }
        public Guid? RefreshTokenId { get; set; }
        public virtual RefreshToken? RefreshToken { get; set; }
    }
}
