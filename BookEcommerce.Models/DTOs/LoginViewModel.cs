using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {

        }

        public LoginViewModel(string? Email, string? Password)
        {
            this.Email = Email;
            this.Password = Password;
        }

        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
