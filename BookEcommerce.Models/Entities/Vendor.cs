using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BookEcommerce.Models.Entities
{
    public class Vendor
    {
        [Key]
        [Column(name: "VendorId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? VendorId { get; set; }

        [Column(name: "FullName")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? FullName { get; set; }

        [Column(name: "StreetAddress")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? StreetAddress { get; set; }

        [Column(name: "Country")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? Country { get; set; }

        [Column(name: "PhoneNumber")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? PhoneNumber { get; set; }

        [Column("AccountId")]
        [Required]
        public ApplicationUser? Account { get; set; }

        //Bank Account
        [Column("BankAccountId")]
        [DataType("varchar")]
        [Required]
        public BankAccount? BankAccount { get; set; }
  

        //Category
    }
}
