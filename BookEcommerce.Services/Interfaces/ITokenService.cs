using BookEcommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateRefreshToken();
        public Task<string> StoreRefreshToken();
        //public string RefreshToken(string AccessToken);
        public string GetUserIdFromToken(string token);
        public string GetUserEmailFromToken(string token);
    }
}
