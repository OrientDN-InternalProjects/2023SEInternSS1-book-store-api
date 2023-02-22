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
        public string CreateAccessToken(List<Claim> Claims);
        public string CreateRefreshToken();
    }
}
