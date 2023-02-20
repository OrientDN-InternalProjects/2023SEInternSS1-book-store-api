using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace BookEcommerce.Models.Entities
{
    public class AccountToken
    {
        [ForeignKey("FK_ACCOUNTTOKEN_REFRESHTOKEN")]
        [Column(name: "RefreshTokenId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public RefreshToken? RefreshToken { get; set; }

        [ForeignKey("FK_ACCOUNTTOKEN_ACCOUNT")]
        [Column(name: "AccountId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public ApplicationUser? Account { get; set; }

        [Column(name: "ExpirationDate")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime? ExpirationDate { get; set; }

    }
}
