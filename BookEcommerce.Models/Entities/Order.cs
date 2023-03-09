using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }
        public string TransferAddress { get; set; }
        public string Message { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusOrder { get; set; }
        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public Guid VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
