using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Category>> SearchCategory(string name)
        {
            return await GetQuery(pr => pr.CategoryName.ToLower().Contains(name.ToLower())).OrderBy(pr => pr.CategoryName).ToListAsync();
        }
    }
}
