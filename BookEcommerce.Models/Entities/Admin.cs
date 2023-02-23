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
        public string? AdminId { get; set; }
        public string? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }
        public string? AccountId { get; set; }
        public ApplicationUser? Account { get; set; }
    }
}
