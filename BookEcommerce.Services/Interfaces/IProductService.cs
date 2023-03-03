using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;

namespace BookEcommerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> AddProduct(ProductRequest req, string Token);
        Task<List<ProductViewModel>> GetAllProduct();
        Task<ProductViewModel> GetProductById(Guid productId);
    }
}
