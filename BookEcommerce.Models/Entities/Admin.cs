using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookEcommerce.Models.Entities
{
    public class Admin
    {
        [Key]
        [Column(name: "AdminId")]
        [DataType("varchar")] 
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? AdminId { get; set; }

        [ForeignKey("FK_ADMIN_BANKACCOUNT")]
        [Column(name: "BankAccountId")]
        [Required]
        public BankAccount? BankAccount { get; set; }
    }
}
