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
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<ProductCategory>> GetProductCategories(Guid categoryId)
        {
            return await GetQuery(ct => ct.CategoryId == categoryId).ToListAsync();
        }
    }
}
