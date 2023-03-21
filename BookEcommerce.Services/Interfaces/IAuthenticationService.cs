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
        public Task<TokenResponse> Login(LoginViewModel loginViewModel);
        public Task<ResponseBase> CustomerRegister(AccountViewModel accountViewModel);
        public Task<ResponseBase> VendorRegister(AccountViewModel accountViewModel);
        public Task<ResponseBase> AdminRegister(AccountViewModel  accountViewModel);
        public Task<TokenResponse> RefreshToken(string Email, TokenViewModel tokenViewModel);
        public UserLoggedResponse GetUserLogged(string token);
    }
}
