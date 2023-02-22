using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BookEcommerce.Models.DAL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration? configuration;
        private readonly ApplicationDbContext applicationDbContext;

        public TokenRepository(IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            this.configuration = configuration;
            this.applicationDbContext = applicationDbContext;
        }

        public string CreateToken(List<Claim> claims)
        {
            SymmetricSecurityKey SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(
                SymmetricSecurityKey,
                SecurityAlgorithms.HmacSha256
            );
            JwtSecurityToken Token = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddSeconds(30)
            );

            var result = new JwtSecurityTokenHandler().WriteToken(Token);
            return result;
        }

        public string CreateRefreshToken()
        {
            var RandomNumber = new byte[64];
            var Generator = RandomNumberGenerator.Create();
            Generator.GetBytes(RandomNumber);
            string Token = Convert.ToBase64String(RandomNumber);
            return Token;
        }

        public async Task StoreRefreshToken(RefreshToken RefreshToken)
        {
            try
            {
                await this.applicationDbContext.RefreshTokens.AddAsync(RefreshToken);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public string RefreshToken(string AccessToken)
        {
            try
            {
                ClaimsPrincipal principal = GetClaimsPrincipal(AccessToken);
                string Email = principal.FindFirstValue(JwtRegisteredClaimNames.Email);

                var NewAccessToken = CreateToken(principal.Claims.ToList());
                return NewAccessToken;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public ClaimsPrincipal GetClaimsPrincipal(string Token)
        {
            TokenValidationParameters TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                RequireExpirationTime = true,
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var principal = TokenHandler.ValidateToken(Token, TokenValidationParameters, out SecurityToken SecurityToken);
            if (SecurityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
