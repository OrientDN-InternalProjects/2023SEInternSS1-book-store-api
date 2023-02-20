using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BookEcommerce.Models.Entities
{
    public class PhoneNumber
    {
        [Key]
        [Column(name: "PhoneNumberId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? PhoneNumberId { get; set; }

        [Column(name: "PhoneNumber")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? Number { get; set; }
    }
}
