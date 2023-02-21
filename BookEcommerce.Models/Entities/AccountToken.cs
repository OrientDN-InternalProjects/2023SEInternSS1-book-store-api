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
        public AccountToken()
        {}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? AccountTokenId { get; set; }

        [ForeignKey("FK_ACCOUNTTOKEN_REFRESHTOKEN")]
        public string? RefreshTokenId { get; set; }
        public RefreshToken? RefreshToken { get; set; }

        [ForeignKey("FK_ACCOUNTTOKEN_ACCOUNT")]
        public string? AccountId { get; set; }
        public ApplicationUser? Account{ get; set; }
        public DateTime ExpirationDate{ get; set; }
    }
}
