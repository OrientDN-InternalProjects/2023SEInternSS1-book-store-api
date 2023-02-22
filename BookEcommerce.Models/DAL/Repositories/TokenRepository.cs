using BookEcommerce.Models.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BookEcommerce.Models.DAL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration? configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;

        }

        public TokenRepository()
        {
            
        }

        public string CreateAccessToken(List<Claim> claims)
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
            throw new NotImplementedException();
        }
    }
}
