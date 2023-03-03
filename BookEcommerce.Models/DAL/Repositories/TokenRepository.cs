using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
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
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256
            );
            JwtSecurityToken token = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddSeconds(30)
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            return result;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            string token = Convert.ToBase64String(randomNumber);
            return token;
        }

        public async Task StoreRefreshToken(RefreshToken RefreshToken)
        {
            await this.applicationDbContext.RefreshTokens.AddAsync(RefreshToken);
        }

        public string RefreshToken(string AccessToken)
        {
            ClaimsPrincipal principal = GetClaimsPrincipal(AccessToken);
            string email = principal.FindFirstValue(JwtRegisteredClaimNames.Email);
            if (email == null)
            {
                return String.Empty;
            }
            var newAccessToken = CreateToken(principal.Claims.ToList());
            return newAccessToken;
        }

        public Guid GetUserIdFromToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var tokenString = handler.ReadToken(token) as JwtSecurityToken;
            Guid userId = new Guid(tokenString!.Claims.First(token => token.Type == "nameid").Value);
            return userId;
        }

        public ClaimsPrincipal GetClaimsPrincipal(string Token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                RequireExpirationTime = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(Token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
