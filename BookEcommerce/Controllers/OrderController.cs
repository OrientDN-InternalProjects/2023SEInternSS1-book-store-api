using BookEcommerce.Models.DTOs.Request;
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
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest orderRequest)
        {
            try
            {
                logger.LogInformation("Start To Add Order");
                var res = await orderService.AddOrder(orderRequest, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
                return StatusCode(StatusCodes.Status201Created, Ok("Add Order Success!!"));
            }
            catch (Exception e)
            {
                logger.LogError("Add Order was Faild!");
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusRequest status, Guid orderId)
        {
            try
            {
                logger.LogInformation("Start To Update Status Order");
                var res = await orderService.ChangeStatusOrder(status,orderId);
                return Ok(res.Message);
            }
            catch (Exception e)
            {
                logger.LogError("Status Order was Faild!");
                return BadRequest(e.Message);
            }
        }
        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            logger.LogInformation("Start To Cancel Status Order");
            var res = await orderService.CancelOrder(orderId);
            if (res.IsSuccess == true)
            {
                return Ok("Đã hủy đơn hàng");
            }
            return BadRequest(res.Message);
        }
    }
}
