using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IRoleRepository
    {
        public Task<bool> CheckRole(string RoleName);
        public Task CreateRole(IdentityRole IdentityRole);
        public Task AddToRole(ApplicationUser ApplicationUser, string Role);
    }
}
