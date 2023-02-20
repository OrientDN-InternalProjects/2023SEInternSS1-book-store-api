using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookEcommerce.Models.Entities
{
    public class Role
    {
        [Key]
        [Column(name: "RoleId")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? RoleId { get; set; }

        [Column(name: "RoleName")]
        [DataType("varchar")]
        [MaxLength(255)]
        [Required]
        public string? RoleName { get; set; }
    }
}
