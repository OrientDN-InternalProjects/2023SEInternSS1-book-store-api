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
        public Task<TokenResponse> Login(LoginViewModel LoginDTO);
        public Task<ResponseBase> CustomerRegister(AccountViewModel AccountDTO);
        public Task<ResponseBase> VendorRegister(AccountViewModel AccountDTO);
        public Task<ResponseBase> AdminRegister(AccountViewModel AccountDTO);
        public Task<ResponseBase> RefreshToken(string Email, TokenViewModel TokenDTO);
    }
}
