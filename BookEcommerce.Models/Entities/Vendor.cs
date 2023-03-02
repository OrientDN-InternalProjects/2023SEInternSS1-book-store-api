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
        public Guid VendorId { get; set; }
        public string? FullName { get; set; }
        public string? StreetAddress { get; set; } 
        public string? Country { get; set; }  
        public string? PhoneNumber { get; set; }

        public string? AccountId { get; set; }
        public virtual ApplicationUser? Account { get; set; }
        public Guid? BankAccountId { get; set; }
        public virtual BankAccount? BankAccount { get; set; }

        //public string ImageId { get; set; }
        public virtual Image? Image { get; set; }
        public virtual ICollection<Product>? Products{ get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
    }
}
