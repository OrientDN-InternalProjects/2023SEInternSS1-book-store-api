using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class AccountViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public AccountViewModel()
        {

        }
        public AccountViewModel(string? UserName, string? Email, string? Password)
        {
            this.UserName = UserName;
            this.Email = Email;
            this.Password = Password;
        }
    }
}
