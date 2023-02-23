using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class AccountDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        
        public AccountDTO()
        {

        }
        public AccountDTO(string? UserName, string? Email, string? Password)
        {
            this.UserName = UserName;
            this.Email = Email;
            this.Password = Password;
        }
    }
}
