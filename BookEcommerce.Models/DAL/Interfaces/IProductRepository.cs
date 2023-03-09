using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProducts(ProductParameters productParameters);
        Task<Product> GetProductById(Guid id);
        Task<List<Product>> SearchProduct(string name);
        Task<List<Product>> SearchProductWithFuzzy(string name);
        Task<List<Product>> GetProductsTopNew();
        Task<List<Product>> GetProductsMostSeller();
    }
}
