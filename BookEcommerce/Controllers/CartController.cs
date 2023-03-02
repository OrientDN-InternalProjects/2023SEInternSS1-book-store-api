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
        public CartController(ICartService cartService, ICustomerService customerService)
        {
            this.cartService = cartService;
            this.customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody]CartRequest cartRequest)
        {
            string AuthHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var customerId = await this.customerService.GetCustomerIdFromToken(AuthHeader);
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
            if (res.Count > 0)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
