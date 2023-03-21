using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Response
{
    public class UserLoggedResponse : ResponseBase
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
