using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookEcommerce.Models.Entities
{
    public class UserAccountRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? UserAccountRoleId { get; set; }



        [ForeignKey("FK_USERACCOUNTROLE_ACCOUNT")]
        public string? AccountId { get; set; }
        public ApplicationUser? Account { get; set; }

        [ForeignKey("FK_USERACCOUNTROLE_ROLE")]
        public string? RoleId { get; set; }
        public string? Role { get; set; }
    }
}
