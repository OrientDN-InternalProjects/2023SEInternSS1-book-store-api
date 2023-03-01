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
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody]CartRequest cartRequest)
        {
            var res = await cartService.AddCart(cartRequest, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
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
