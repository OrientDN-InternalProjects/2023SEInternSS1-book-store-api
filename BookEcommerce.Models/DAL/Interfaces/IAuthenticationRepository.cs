using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.DTOs.Request;
using Microsoft.AspNetCore.Identity;
using BookEcommerce.Models.Entities;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<IdentityResult> RegisterCustomer(ApplicationUser ApplicationUser, string Password);
        public Task<IdentityResult> RegisterVendor(ApplicationUser ApplicationUser, string password);
        public Task<IdentityResult> CreateAdmin(ApplicationUser applicationUser);
        public Task<string> Login(LoginDTO LoginDTO);
    }
}
