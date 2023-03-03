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
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration!["JWT:Key"]));
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256
            );
            var token = new JwtSecurityToken(
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
            var token = Convert.ToBase64String(randomNumber);
            return token;
        }

        public async Task StoreRefreshToken(RefreshToken refreshToken)
        {
            await this.applicationDbContext.RefreshTokens.AddAsync(refreshToken);
        }

        public string RefreshToken(string accessToken)
        {
            var principal = GetClaimsPrincipal(accessToken);
            var email = principal.FindFirstValue(JwtRegisteredClaimNames.Email);
            if (email == null)
            {
                return String.Empty;
            }
            var newAccessToken = CreateToken(principal.Claims.ToList());
            return newAccessToken;
        }

        public Guid GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.ReadToken(token) as JwtSecurityToken;
            var userId = new Guid(tokenString!.Claims.First(token => token.Type == "nameid").Value);
            return userId;
        }

        public ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration!["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                RequireExpirationTime = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
