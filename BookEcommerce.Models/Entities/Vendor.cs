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
        public Vendor()
        {
            Categories = new HashSet<Category>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? VendorId { get; set; }
        public string? FullName { get; set; }
        public string? StreetAddress { get; set; } 
        public string? Country { get; set; }  
        public string? PhoneNumber { get; set; }

        public string? AccountId { get; set; }
        public ApplicationUser? Account { get; set; }
        public string? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }

        //public string ImageId { get; set; }
        public Image? Image { get; set; }
        public ICollection<Product>? Products{ get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<Category>? Categories { get; set; }
    }
}
