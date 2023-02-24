using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "VENDOR")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ProductRequest request)
        {
            var res = await productService.AddProduct(request);
            if(res.IsSuccess)
            {
                return Ok("Add Product Success!!");
            }
            return BadRequest(res.Message);
        }
    }
}
