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
using BookEcommerce.Models.DTOs;
using System.Web;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class AuthenticationRepository : Repository<ApplicationUser>, IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser>? userManager;
        private readonly ITokenRepository? tokenRepository;
        private readonly IRoleRepository? roleRepository;
        private readonly SignInManager<ApplicationUser>? signInManager;
        //private readonly ISendMailRepository sendMailRepository;
        //private readonly ApplicationDbContext? applicationDbContext;
        //private readonly ISendMailRepository sendMailRepository;

        public AuthenticationRepository(
            UserManager<ApplicationUser> userManager,
            ITokenRepository tokenRepository,
            IRoleRepository roleRepository,
            SignInManager<ApplicationUser>? signInManager,
            //ISendMailRepository sendMailRepository
            DbFactory dbFactory
            //ISendMailRepository sendMailRepository
        ) : base(dbFactory)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.roleRepository = roleRepository;
            this.signInManager = signInManager;
            //this.sendMailRepository = sendMailRepository;
            //this.sendMailRepository = sendMailRepository;
        }

        public async Task<IdentityResult> CreateAdmin(ApplicationUser ApplicationUser, string password)
        {
            var Result = await this.userManager!.CreateAsync(ApplicationUser, password);
            if (!Result.Succeeded) return Result;
            var RoleExisted = await this.roleRepository!.CheckRole(Roles.ADMIN);
            if (!RoleExisted)
            {
                await this.roleRepository.CreateRole(new IdentityRole(Roles.ADMIN));
            }
            await this.roleRepository.AddToRole(ApplicationUser, Roles.ADMIN);
            return Result;
        }

        //public async Task<IdentityResult> CreateAdmin(ApplicationUser ApplicationUser)
        //{

        //    var Admin = await this.userManager.CreateAsync(ApplicationUser, Password.PASSWORD);
        //}

        public async Task<TokenViewModel> Login(LoginViewModel LoginDTO)
        {
            var User = await this.GetUserByEmail(LoginDTO.Email!);
            if (User == null) return null!;
            //if (!await this.userManager.CheckPasswordAsync(User, LoginDTO.Password)) return null;
            SignInResult result = await this.signInManager!.PasswordSignInAsync(User, LoginDTO.Password, false, false);
            if (!result.Succeeded) return null!;

            List<Claim> claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.NameId, User.Id),
            new Claim(JwtRegisteredClaimNames.Email, LoginDTO.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var RoleManager = await this.userManager!.GetRolesAsync(User);
            foreach (var role in RoleManager)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var AccessToken = tokenRepository!.CreateToken(claims);
            var RefreshToken = tokenRepository!.CreateRefreshToken();
            return new TokenViewModel
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
        }

        public async Task<IdentityResult> RegisterCustomer(ApplicationUser ApplicationUser, string password)
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

        public async Task<IdentityResult> RegisterVendor(ApplicationUser ApplicationUser, string password)
        {
            IdentityResult result = await this.userManager!.CreateAsync(ApplicationUser, password);
            if (!result.Succeeded) return result;
            var RoleExisted = await this.roleRepository!.CheckRole(Roles.VENDOR);
            if (!RoleExisted)
            {
                await this.roleRepository.CreateRole(new IdentityRole(Roles.VENDOR));
            }
            await this.roleRepository.AddToRole(ApplicationUser, Roles.VENDOR);
            return result;      
        }

        public async Task<string> RefreshToken(string Email, TokenViewModel tokenViewModel)
        {           
            var user = await this.userManager!.FindByEmailAsync(Email);
            if (user == null)
            {
                return null!;
            }
            if (user.RefreshTokenId != Guid.Parse(tokenViewModel.RefreshToken)) return null!;
            string Token = this.tokenRepository!.RefreshToken(tokenViewModel.AccessToken!);
            return Token;            
        }

        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {           
            var result = await this.userManager!.FindByEmailAsync(Email);
            return result;           
        }

        public async Task Revoke(string Email)
        {        
            var user = await this.userManager!.FindByEmailAsync(Email);
            if (user != null)
            {
                user.RefreshTokenId = null;
            }   
        }
    }
}
