using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookEcommerce.Models.Entities
{
    public class Customer
    {
        [Key]
        [Column(name: "CustomerId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? CustomerId { get; set; }

        [Column(name: "FullName")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? FullName { get; set; }

        [ForeignKey("FK_CUSTOMER_ADDRESS")]
        [Column(name: "AddressId")]
        [DataType("varchar")]
        [Required]
        public List<Address>? Addresses { get; set; }

        [Column(name: "PhoneNumberId")]
        [DataType("varchar")]
        [Required]
        public List<PhoneNumber>? PhoneNumbers { get; set; }

        [Column(name: "AccountId")]
        [DataType("varchar")]
        [Required]
        [ForeignKey("FK_CUSTOMER_ACCOUNT")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
