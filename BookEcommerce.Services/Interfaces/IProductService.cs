using BookEcommerce.Models.DAL.Repositories;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;

namespace BookEcommerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> AddProduct(ProductRequest req, string token);
        Task<List<ProductViewModel>> GetAllProduct();
        Task<List<ProductViewModel>> GetProductMostSellProduct();
        Task<List<ProductViewModel>> GetProductTopNew();
        Task<ProductViewModel> GetProductById(Guid productId);
    }
}
