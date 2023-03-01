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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        { 
            var res = await productService.GetProductById(productId);
            if(res.IsSuccess)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var res = await productService.GetAllProduct();
            return Ok(res);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "VENDOR")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ProductRequest request)
        {
            string AuthHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var res = await productService.AddProduct(request, AuthHeader);
            if(res.IsSuccess)
            {
                return Ok("Add Product Success!!");
            }
            return BadRequest(res.Message);
        }
    }
}
