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
        public BankAccount()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid? BankAccountId { get; set; }
        public virtual string? BankAccountName { get; set; }
        public virtual string? BankAccountCode { get; set; }
        public virtual DateTime? CreatedDate { get; set; }

        public virtual Guid? BankProviderId { get; set; }
        public virtual BankProvider? BankProvider { get; set; }

    }
}
