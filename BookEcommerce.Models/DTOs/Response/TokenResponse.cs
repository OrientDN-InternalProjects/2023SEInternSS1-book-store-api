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
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public bool? IsActive { get; set; }
        public TokenResponse() : base()
        {

        }
        public TokenResponse(bool IsSuccess, string Message, string AccessToken, string RefreshToken) : base(IsSuccess, Message)
        {
            this.AccessToken = AccessToken;
            this.RefreshToken = RefreshToken;
        }
    }
}
