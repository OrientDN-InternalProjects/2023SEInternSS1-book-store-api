using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class MailSettings
    {
        //public MailSettings(string? mail, string? displayName, string? password, string? host, int port)
        //{
        //    Mail = mail;
        //    DisplayName = displayName;
        //    Password = password;
        //    Host = host;
        //    Port = port;
        //}

        public string Mail { get => "huy2002109@gmail.com"; }
        public string DisplayName { get => "BookCommerce"; }
        public string Password { get => "askronkvelldgtke"; }
        public string Host { get => "smtp.gmail.com"; }
        public int Port { get => 587; }
    }
}
