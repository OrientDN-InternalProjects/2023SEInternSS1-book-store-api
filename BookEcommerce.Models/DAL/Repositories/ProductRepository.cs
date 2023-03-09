using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public ProductRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }



        public async Task<Product> GetProductById(Guid id)
        {
            return await GetQuery(pr => pr.ProductId.Equals(id)).SingleAsync();
        }

        public async Task<List<Product>> SearchProduct(string name)
        {
            return await GetQuery(pr => pr.ProductName.ToLower().Contains(name.ToLower())).OrderBy(pr => pr.ProductName).ToListAsync();
        }
        public async Task<List<Product>> SearchProductWithLike(string name)
        {
            return await GetQuery(p => EF.Functions.Like(sWhitespace.Replace(p.ProductName!,""), sWhitespace.Replace($"%{name}%",""))).ToListAsync();
            //return await GetQuery(e => GetDbContext().FuzzySearch(e.ProductName.ToLower()) == GetDbContext().FuzzySearch(name.ToLower())).ToListAsync();
        }
    }
}
