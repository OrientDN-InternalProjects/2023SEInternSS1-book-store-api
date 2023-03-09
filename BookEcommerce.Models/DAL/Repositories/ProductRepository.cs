using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ILogger<ProductRepository> logger;
        public ProductRepository(DbFactory dbFactory, ILogger<ProductRepository> logger) : base(dbFactory)
        {
            this.logger = logger;
        }

        public IEnumerable<Product> GetProducts(ProductParameters productParameters)
        {
            return FindAll().OrderBy(on => on.ProductName).Skip((productParameters.PageNumber - 1) * productParameters.PageSize).Take(productParameters.PageSize).ToList();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await GetQuery(pr => pr.ProductId.Equals(id)).SingleAsync();
        }

        public async Task<List<Product>> SearchProduct(string name)
        {
            return await GetQuery(pr => pr.ProductName!.ToLower().Contains(name.ToLower())).OrderBy(pr => pr.ProductName).ToListAsync();
        }

        public async Task<List<Product>> SearchProductWithFuzzy(string name)
        {
            var products = await GetAll();
            var listProducts = new List<Product>();
            foreach(var item in products)
            {
                var ratioWeighted = Fuzz.WeightedRatio(name, item.ProductName);
                if (ratioWeighted > 60)
                {
                    listProducts.Add(item);
                };
            }
            return listProducts;
        }

        public async Task<List<Product>> GetProductsMostSeller()
        {
            var products = await GetAll();
            var topFiveProduct = products.OrderByDescending(p => p.Sold).Take(5).ToList();
            return topFiveProduct;
        }

        public async Task<List<Product>> GetProductsTopNew()
        {
            var products = await GetAll();
            var topFiveProduct = products.OrderByDescending(p => p.DateCreated).Take(5).ToList();
            return topFiveProduct;
        }
    }
}
