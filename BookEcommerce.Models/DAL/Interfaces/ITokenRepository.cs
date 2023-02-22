using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface ITokenRepository
    {
        public string CreateToken(List<Claim> Claims);
        public string CreateRefreshToken();
        public Task StoreRefreshToken(RefreshToken RefreshToken);
        public string RefreshToken(string AccessToken);
        public ClaimsPrincipal GetClaimsPrincipal(string Token);
    }
}
