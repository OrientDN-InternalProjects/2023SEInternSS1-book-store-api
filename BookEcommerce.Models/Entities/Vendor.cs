using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookEcommerce.Models.Entities
{
    public class Vendor
    {
        [Key]
        public string? VendorId { get; set; }
        public string? FullName { get; set; }
        public string? StreetAddress { get; set; } 
        public string? Country { get; set; }  
        public string? PhoneNumber { get; set; }

        [ForeignKey("FK_VENDOR_ACCOUNT")]
        public string? AccountId { get; set; }
        public ApplicationUser? Account { get; set; }

        [ForeignKey("FK_VENDOR_BANKACCOUNT")]
        public string? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }

        [ForeignKey("FK_VENDOR_CATEGORY")]
        public List<Category>? Category { get; set; }
    }
}
