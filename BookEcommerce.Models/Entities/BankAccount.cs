using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BookEcommerce.Models.Entities
{
    public class BankAccount
    {
        [Key]
        [Column(name: "BankAccountId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? BankAccountId { get; set; }

        [Column(name: "BankAccountName")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? BankAccountName { get; set; }

        [Column(name: "BankAccountCode")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? BankAccountCode { get; set; }

        [Column(name: "CreatedDate")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedDate { get; set; }

        //[Column("BankProviderId")]
        [Column(name: "BankProviderId")]
        [DataType("varchar")]
        [Required]
        public BankProvider? BankProvider { get; set; }
    }
}
