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

        public async Task<IdentityResult> CreateAdmin(ApplicationUser admin, string password)
        {
            var result = await this.userManager!.CreateAsync(admin, password);
            if (!result.Succeeded) return result;
            var roleExisted = await this.roleRepository!.CheckRole(Roles.ADMIN);
            if (!roleExisted)
            {
                await this.roleRepository.CreateRole(new IdentityRole(Roles.ADMIN));
            }
            await this.roleRepository.AddToRole(admin, Roles.ADMIN);
            return result;
        }

        //public async Task<IdentityResult> CreateAdmin(ApplicationUser ApplicationUser)
        //{

        //    var Admin = await this.userManager.CreateAsync(ApplicationUser, Password.PASSWORD);
        //}

        public async Task<TokenViewModel> Login(LoginViewModel loginDTO)
        {
            var user = await this.GetUserByEmail(loginDTO.Email!);
            if (user == null) return null!;
            //if (!await this.userManager.CheckPasswordAsync(User, LoginDTO.Password)) return null;
            SignInResult result = await this.signInManager!.PasswordSignInAsync(user, loginDTO.Password, false, false);
            if (!result.Succeeded) return null!;

            List<Claim> claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, loginDTO.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roleManager = await this.userManager!.GetRolesAsync(user);
            foreach (var role in roleManager)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var accessToken = tokenRepository!.CreateToken(claims);
            var refreshToken = tokenRepository!.CreateRefreshToken();
            return new TokenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<IdentityResult> RegisterCustomer(ApplicationUser applicationUser, string password)
        {
            IdentityResult result = await this.userManager!.CreateAsync(applicationUser, password);
            if (!result.Succeeded) return result;
            var roleExisted = await this.roleRepository!.CheckRole(Roles.CUSTOMER);
            if (!roleExisted)
            {
                await this.roleRepository.CreateRole(new IdentityRole(Roles.CUSTOMER));
            }
            await this.roleRepository.AddToRole(applicationUser, Roles.CUSTOMER);
            return result;
        }

        public async Task<IdentityResult> RegisterVendor(ApplicationUser applicationUser, string password)
        {
            IdentityResult result = await this.userManager!.CreateAsync(applicationUser, password);
            if (!result.Succeeded) return result;
            var roleExisted = await this.roleRepository!.CheckRole(Roles.VENDOR);
            if (!roleExisted)
            {
                await this.roleRepository.CreateRole(new IdentityRole(Roles.VENDOR));
            }
            await this.roleRepository.AddToRole(applicationUser, Roles.VENDOR);
            return result;      
        }

        public async Task<string> RefreshToken(string email, TokenViewModel tokenViewModel)
        {           
            var user = await this.userManager!.FindByEmailAsync(email);
            if (user == null)
            {
                return null!;
            }
            if (user.RefreshToken!.Token != tokenViewModel.RefreshToken!) return null!;
            string token = this.tokenRepository!.RefreshToken(tokenViewModel.AccessToken!);
            return token;            
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {           
            var result = await this.userManager!.FindByEmailAsync(email);
            return result;           
        }

        public async Task Revoke(string email)
        {        
            var user = await this.userManager!.FindByEmailAsync(email);
            if (user != null)
            {
                user.RefreshTokenId = null;
            }   
        }
    }
}
