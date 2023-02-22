using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookEcommerce.Models.Entities
{
    public class RefreshToken
    {
        public RefreshToken()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? RefreshTokenId { get; set; }
        public string? Token { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
