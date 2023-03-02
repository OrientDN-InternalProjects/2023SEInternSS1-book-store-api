using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookEcommerce.Models.Entities
{
    public class Admin
    {
        public Admin()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AdminId { get; set; }
        public Guid? BankAccountId { get; set; }
        public virtual BankAccount? BankAccount { get; set; }
        public virtual string? AccountId { get; set; }
        public virtual ApplicationUser? Account { get; set; }
    }
}
