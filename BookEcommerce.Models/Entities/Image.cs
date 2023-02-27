using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Image
    {
        public Image()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? ImageId { get; set; }
        public string? ImageURL { get; set; }
        public Guid? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public Guid? VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }
        public Guid? ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
