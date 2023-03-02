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
        private readonly ILogger<ProductController> logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                logger.LogInformation("Start Get Product! ");
                var res = await productService.GetProductById(productId);
                return Ok(res);
            }
            catch (Exception e)
            {
                logger.LogError("Get Product Fail!");
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            logger.LogInformation("Start All Product! ");
            var res = await productService.GetAllProduct();
            return Ok(res);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "VENDOR")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ProductRequest request)
        {
            logger.LogInformation("Start Get Product! ");
            string AuthHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var res = await productService.AddProduct(request, AuthHeader);
            if(res.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created,Ok("Add Product Success!!"));
            }
            return BadRequest(res.Message);
        }
    }
}
