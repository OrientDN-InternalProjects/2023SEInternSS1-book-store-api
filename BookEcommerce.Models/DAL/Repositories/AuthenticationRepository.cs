using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Response.Base;
using Microsoft.AspNetCore.Identity;
using BookEcommerce.Models.Entities;
using Microsoft.Extensions.Configuration;
using BookEcommerce.Models.DTOs.Request;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BookEcommerce.Models.DAL.Constants;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser>? userManager;
        private readonly ITokenRepository? tokenRepository;
        private readonly IRoleRepository? roleRepository;

        public AuthenticationRepository(
            UserManager<ApplicationUser> userManager,
            ITokenRepository tokenRepository,
            IRoleRepository roleRepository
        )
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.roleRepository = roleRepository;
        }

        public Task<IdentityResult> CreateAdmin(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        //public async Task<IdentityResult> CreateAdmin(ApplicationUser ApplicationUser)
        //{

        //    var Admin = await this.userManager.CreateAsync(ApplicationUser, Password.PASSWORD);
        //}

        public async Task<string> Login(LoginDTO LoginDTO)
        {
            var User = await this.userManager!.FindByEmailAsync(LoginDTO.Email);
            if (User == null) return "User does not exist";
            if (!await this.userManager.CheckPasswordAsync(User, LoginDTO.Password)) return "Username or Password invalid";

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, LoginDTO.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var RoleManager = await this.userManager.GetRolesAsync(User);
            foreach (var role in RoleManager)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = tokenRepository!.CreateAccessToken(claims);
            return token;
        }

        public async Task<IdentityResult> RegisterCustomer(ApplicationUser ApplicationUser, string password)
        {
            try
            {
                IdentityResult result = await this.userManager!.CreateAsync(ApplicationUser, password);
                if (!result.Succeeded) return result;
                var RoleExisted = await this.roleRepository!.CheckRole(Roles.CUSTOMER);
                if (!RoleExisted)
                {
                    await this.roleRepository.CreateRole(new IdentityRole(Roles.CUSTOMER));
                }
                await this.roleRepository.AddToRole(ApplicationUser, Roles.CUSTOMER);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IdentityResult> RegisterVendor(ApplicationUser ApplicationUser, string password)
        {
            try
            {
                IdentityResult result = await this.userManager!.CreateAsync(ApplicationUser, password);
                if (!result.Succeeded) return result;
                var RoleExisted = await this.roleRepository!.CheckRole(Roles.VENDOR);
                if (!RoleExisted)
                {
                    await this.roleRepository.CreateRole(new IdentityRole(Roles.VENDOR));
                }
                await this.roleRepository.AddToRole(ApplicationUser, password);
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
