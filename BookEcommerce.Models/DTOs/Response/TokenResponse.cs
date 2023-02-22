using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Response
{
    public class TokenResponse : ResponseBase
    {
        public string? Token { get; set; }
        public TokenResponse() : base()
        {

        }
        public TokenResponse(bool IsSuccess, string Message, string Token) : base(IsSuccess, Message)
        {
            this.Token = Token;
        }
    }
}
