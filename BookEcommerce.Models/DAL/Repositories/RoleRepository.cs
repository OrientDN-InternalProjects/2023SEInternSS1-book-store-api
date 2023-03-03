using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task AddToRole(ApplicationUser ApplicationUser, string Role)
        {
            await this.userManager.AddToRoleAsync(ApplicationUser, Role);
        }

        public async Task<bool> CheckRole(string RoleName)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(RoleName);
            var existed = !roleExist ? false : true;
            return existed;
        }

        public async Task CreateRole(IdentityRole IdentityRole)
        {
            await this.roleManager.CreateAsync(IdentityRole);
        }
    }
}
