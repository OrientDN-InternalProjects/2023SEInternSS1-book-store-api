using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        private readonly ICustomerService customerService;
        public PaymentController(IPaymentService paymentService, ICustomerService customerService)
        {
            this.paymentService = paymentService;
            this.customerService = customerService;
        }
        [HttpGet("/payment")]
        [Authorize(Roles = "CUSTOMER", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Payment([FromQuery] Guid orderId)
        {
            //string AuthHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            //var email = await this.customerService.GetCustomerEmailFromToken(AuthHeader);
            var result = await this.paymentService.CreatePaymentWithPaypal(orderId, null);
            if (result.IsSuccess)
            {
                return Ok(new PaymentResponse
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                    RedirectUrl = result.RedirectUrl
                });
            }
            return NotFound(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }
    }
}
