using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Services.Interfaces;
using FuzzySharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            logger.LogInformation("Start Get Product! ");
            var res = await productService.GetProductById(productId);
            if (res.IsSuccess)
            {
                logger.LogInformation("Can find product! ");
                return Ok(res);
            }
            logger.LogError("Get product fail!");
            return NotFound(res.Message);
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
            string authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var res = await productService.AddProduct(request, authHeader);
            if (res.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created,res);
            }
            logger.LogError("Add Product was failed!");
            return BadRequest(res.Message);
        }

        [HttpGet("all-product-paging")]
        public IActionResult GetOwners([FromQuery] ProductParameters productParameters)
        {
            var products = productService.GetProducts(productParameters);
            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };
            logger.LogInformation($"Returned {products.TotalCount} owners from database.");
            return Ok(products);
        }
    }
}
