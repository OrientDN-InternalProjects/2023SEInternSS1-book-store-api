using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class OrderDetail
    {
        public OrderDetail()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public string? OrderId { get; set; }
        public Order? Order { get; set; }
        
        public string? ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }

    }
}
