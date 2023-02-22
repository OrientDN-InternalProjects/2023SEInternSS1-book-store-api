using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public partial class ProductVariant
    {
        public ProductVariant()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? ProductVariantId { get; set; }
        public string? ProductVariantName { get; set; }
        public int Quantity { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual ProductPrice? ProductPrice  { get; set; }
        public string? OrderDetailId { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }

    }
}
