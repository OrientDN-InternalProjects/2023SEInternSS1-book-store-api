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
        public async Task AddToRole(ApplicationUser applicationUser, string role)
        {
            await this.userManager.AddToRoleAsync(applicationUser, role);
        }

        public async Task<bool> CheckRole(string roleName)
        {
            return await this.roleManager.RoleExistsAsync(roleName);
        }

        public async Task CreateRole(IdentityRole identityRole)
        {
            await this.roleManager.CreateAsync(identityRole);
        }
    }
}
