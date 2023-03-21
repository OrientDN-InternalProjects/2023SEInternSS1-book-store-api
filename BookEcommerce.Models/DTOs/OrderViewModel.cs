using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class OrderViewModel : ResponseBase
    {
        public Guid CustomerId { get; set; }
        public string? TransferAddress { get; set; }
        public string? Message { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
