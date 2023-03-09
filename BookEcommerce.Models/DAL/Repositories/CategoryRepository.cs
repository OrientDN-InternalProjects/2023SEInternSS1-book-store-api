using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using FuzzySharp;
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
            var categories = await GetAll();
            var listProducts = new List<Category>();
            foreach (var item in categories)
            {
                var ratio = Fuzz.Ratio(name, item.CategoryName);
                if (ratio > 35)
                {
                    listProducts.Add(item);
                };
            }
            return listProducts;
        }
    }
}
