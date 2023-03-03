using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class MailSendingViewModel
    {
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? HtmlMessage { get; set; }
    }
}
