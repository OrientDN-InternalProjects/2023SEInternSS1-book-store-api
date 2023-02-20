using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookEcommerce.Models.Entities
{
    public class BankProvider
    {
        [Key]
        [Column(name: "BankProviderId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? BankProviderId { get; set; }

        [Column(name: "BankProviderName")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? BankProviderName { get; set; }

        [Column(name: "BankProviderShortName")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? BankProviderShortName { get; set; }
    }
}
