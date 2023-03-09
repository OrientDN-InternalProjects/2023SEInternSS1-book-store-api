﻿using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Services.Interfaces;
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

        [HttpPost]
        public async Task<IActionResult> AddProdutOfShop([FromBody] ProductRequest request)
        {
            logger.LogInformation("Start Get Product! ");
            var res = await productService.AddProduct(request);
            if(res.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created,Ok(res.Message));
            }
            logger.LogError("Add Product was failed!");
            return BadRequest(res.Message);
        }
    }
}
