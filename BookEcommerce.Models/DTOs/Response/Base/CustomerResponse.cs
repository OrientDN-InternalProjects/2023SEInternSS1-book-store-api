using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Response.Base
{
    public class CustomerResponse : ResponseBase
    {
        public string? CustomerId { get; set; }
        public string? CustomerFullName { get; set; }
    }
}
