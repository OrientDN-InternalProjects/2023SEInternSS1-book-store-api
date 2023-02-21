using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookEcommerce.Models.Entities
{
    public class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? BankAccountId { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankAccountCode { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("FK_BANKACCOUNT_BANK_PROVIDER")]
        public string? Bank_Provider_Id { get; set; }
        public BankProvider? BankProvider { get; set; }

    }
}
