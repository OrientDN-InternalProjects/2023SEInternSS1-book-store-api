using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookEcommerce.Models.Entities
{
    public class RefreshToken
    {
        [Key]
        [Column(name: "RefreshTokenId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? RefreshTokenId { get; set; }

        [Column(name: "RefreshToken")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? Token { get; set; }

        [Column(name: "AccountTokenId")]
        [DataType("varchar")]
        [Required]
        [ForeignKey("FK_REFRESHTOKEN_ACCOUNTTOKEN")]
        public AccountToken? AccountToken { get; set; }
    }
}
