using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private ILogger<SearchController> logger;

        private readonly IProductRepository productRepository;
        public SearchController(ISearchService searchService, ILogger<SearchController> logger, IProductRepository productRepository)
        {
            this.searchService = searchService;
            this.logger = logger;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById([FromBody] SearchRequest searchRequest)
        {
            logger.LogInformation("Start Search Product! ");
            var res = await searchService.SearchProductByNameAndType(searchRequest);
            //if (res.IsSuccess)
            //{
            //    logger.LogInformation("Can find product! ");
            //    return Ok(res);
            //}
            //logger.LogError("Get product fail!");
            //return NotFound(res.Message);
            return Ok(res);
        }

        //[HttpGet]
        //public async Task<IActionResult> Search([FromQuery] string name)
        //{
        //    logger.LogInformation("Start Search Product! ");
        //    var res = await searchService.SearchProduct(name);
        //    //if (res.IsSuccess)
        //    //{
        //    //    logger.LogInformation("Can find product! ");
        //    //    return Ok(res);
        //    //}
        //    //logger.LogError("Get product fail!");
        //    //return NotFound(res.Message);
        //    return Ok(res);
        //}
    }
}
