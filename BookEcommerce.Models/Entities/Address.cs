using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BookEcommerce.Models.Entities
{
    public class Address
    {
        [Key]
        [Column(name: "AddressId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? AddressId { get; set; }

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
    }
}
