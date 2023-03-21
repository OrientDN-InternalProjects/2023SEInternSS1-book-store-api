using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ILogger<OrderController> logger;
        private readonly ICustomerService customerService;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest orderRequest)
        {     
            string authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var customerId = await this.customerService.GetCustomerIdFromToken(authHeader);
            logger.LogInformation("Start To Add Order");
            var res = await orderService.AddOrder(orderRequest, customerId);
            if (res.IsSuccess)
            { 
                logger.LogInformation("Add Order Success!");
                return StatusCode(StatusCodes.Status201Created, res);
            }
            logger.LogError("Add Order was failed!");
            return BadRequest(
                new OrderResponse
                {
                    IsSuccess = false,
                    Message = res.Message
                });              
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusRequest status, Guid orderId)
        {
            logger.LogInformation("Start To Update Status Order");
            var res = await orderService.ChangeStatusOrder(status, orderId);
            if (res.IsSuccess)
            {
                logger.LogInformation("Order status has been updated");
                return Ok(res.Message);
            }
            logger.LogError("Status Order was failed!");
            return BadRequest(res.Message);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            logger.LogInformation("Start To Get Order!");
            var res = await orderService.GetOrder(orderId);
            if (res.IsSuccess)
            {
                logger.LogInformation("Get Order Success!");
                return Ok(res);
            }
            logger.LogError("Get Order was failed!");
            return BadRequest(
                new OrderResponse
                {
                    IsSuccess = false,
                    Message = res.Message
                });
        }
    }
}
