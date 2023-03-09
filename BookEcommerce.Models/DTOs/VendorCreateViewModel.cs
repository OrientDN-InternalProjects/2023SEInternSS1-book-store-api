using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class VendorCreateViewModel
    {
        public string? FullName { get; set; }
        public string? StreetAddress { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public string? BankAccountId { get; set; }

    }
}
