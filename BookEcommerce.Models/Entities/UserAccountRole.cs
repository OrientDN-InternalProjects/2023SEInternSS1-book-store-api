using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BookEcommerce.Models.Entities
{
    public class UserAccountRole
    {
        [ForeignKey("FK_ACCOUNTROLE_ROLE")]
        [Column(name: "AccountId")]
        [DataType("varchar")]
        [Required]
        public List<ApplicationUser>? AccountId { get; set; }

        [ForeignKey("FK_ACCOUNTROLE_ACCOUNT")]
        [Column(name: "RoleId")]
        [DataType("varchar")]
        [Required]
        public List<Role>? Roles { get; set; }
    }
}
