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
        private readonly ILogger<PaymentController> logger;
        public PaymentController(IPaymentService paymentService, ICustomerService customerService, ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            this.customerService = customerService;
            this.logger = logger;
        }

        [HttpGet("/payment")]
        public async Task<IActionResult> Payment([FromQuery] Guid orderId, string? paymentId)
        {
            if (paymentId == null)
            {
                var res = await this.paymentService.CreatePaymentWithPaypal(orderId, paymentId!, null);
                return Ok(res);
            }
            return BadRequest();
        }
        [HttpGet("/execute")]
        public async Task<IActionResult> Execute([FromQuery] Guid orderId, string? payerId, string? paymentId)
        {
            var result = this.paymentService.ExecutePayment(orderId, payerId!, paymentId!);
            logger.LogInformation(paymentId);
            return Ok(new PaymentResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Transaction = result.Transaction
            });
        }
    }
}

