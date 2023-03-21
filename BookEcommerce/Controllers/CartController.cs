using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly ICustomerService customerService;
        private readonly ILogger<CartController> logger;
        public CartController(ICartService cartService, ICustomerService customerService, ILogger<CartController> logger)
        {
            this.logger = logger;
            this.cartService = cartService;
            this.customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody]CartRequest cartRequest)
        {
            string authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var customerId = await this.customerService.GetCustomerIdFromToken(authHeader);
            if (cartRequest.Quantity <= 0)
            {
                logger.LogError("Enter value 0!");
                return BadRequest("Can't enter numbers less than 0 when add product to cart!");
            }
            var res = await cartService.AddCart(cartRequest, customerId);
            if (res.IsSuccess)
            {
                return Ok(res.Message);
            }
            return BadRequest(res.Message);
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCartByIdCustomer(Guid customerId)
        {
            var res = await cartService.GetCart(customerId);
            return Ok(res);
        }
    }
}
