using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class DetailsRequest
    {
        public Guid ShopId { get; set; }
        public List<OrderDetailRequest>? OrderDetailRequests { get; set; }
    }
}
