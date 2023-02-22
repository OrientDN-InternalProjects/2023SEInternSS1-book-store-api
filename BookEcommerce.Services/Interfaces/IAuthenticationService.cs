using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<TokenResponse> Login(LoginDTO LoginDTO);
        public Task<ResponseBase> CustomerRegister(AccountDTO AccountDTO);
        public Task<ResponseBase> VendorRegister(AccountDTO AccountDTO);
        public Task<ResponseBase> AdminRegister(AccountDTO AccountDTO);
        public Task<ResponseBase> RefreshToken(string Email, TokenDTO TokenDTO);
    }
}
