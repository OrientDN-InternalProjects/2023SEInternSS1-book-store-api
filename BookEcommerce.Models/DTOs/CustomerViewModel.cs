using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class CustomerViewModel
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public string? ImageId { get; set; }
    }
}
