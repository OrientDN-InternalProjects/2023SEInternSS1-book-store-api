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
        [Key]
        public string? AdminId { get; set; }

        [ForeignKey("FK_ADMIN_BANKACCOUNT")]
        public string? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }
    }
}
