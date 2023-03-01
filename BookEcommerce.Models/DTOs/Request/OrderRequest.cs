using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class OrderRequest
    {
        public string? TransferAddress { get; set; }
        public Guid PaymentId { get; set; }
        public string? Message { get; set; }
        public string? StatusOrder { get; set; }
        public List<DetailsRequest>? Details{ get; set; }
    }
}
