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

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            this.searchService = searchService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SearchProduct([FromQuery] SearchRequest searchRequest)
        {
            logger.LogInformation("Start Search Product! ");
            var res = await searchService.SearchProductByNameAndType(searchRequest);
            return Ok(res);
        }
    }
}
