using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookEcommerce.Models.DTOs.Response.Base;
using Microsoft.AspNetCore.Identity;
using BookEcommerce.Models.Entities;
using BookEcommerce.Models.DTOs;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IAuthenticationRepository : IRepository<ApplicationUser>
    {
        public Task<ApplicationUser> GetUserByEmail(string Email);
        public Task<IdentityResult> RegisterCustomer(ApplicationUser ApplicationUser, string Password);
        public Task<IdentityResult> RegisterVendor(ApplicationUser ApplicationUser, string password);
        public Task<IdentityResult> CreateAdmin(ApplicationUser applicationUser, string password);
        public Task<string> Login(LoginDTO LoginDTO);
        public Task<string> RefreshToken(string Email, TokenDTO TokenDTO);
        public Task Revoke(string Email);
        //public Task ForgotPassword(string Email, string OldPassword, string NewPassword);
        //public Task AddRefreshToken(string TokenId);

    }
}
