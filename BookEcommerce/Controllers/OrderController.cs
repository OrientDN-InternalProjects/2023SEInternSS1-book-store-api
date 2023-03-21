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
                return StatusCode(StatusCodes.Status201Created, Ok("Add Order Success!!"));
            }
            logger.LogError("Add Order was Faild!");
            return BadRequest(res.Message);    
        }
            }
            catch (Exception e)
            {
                logger.LogError("Add Order was Failed!");
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusRequest status, Guid orderId)
        {
            logger.LogInformation("Start To Update Status Order");
        }
            if (res.IsSuccess)
            {
                logger.LogInformation("Order status has been updated");
                return Ok(res.Message);
            }
            logger.LogError("Status Order was Faild!");
            return BadRequest(res.Message);
        }
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
>>>>>>>>> Temporary merge branch 2
        }
        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            logger.LogInformation("Start To Cancel Status Order");
            var res = await orderService.CancelOrder(orderId);
            if (res.IsSuccess == true)
            {
                logger.LogInformation("Order status has been cancel!");
                return Ok("Cancel Order success!");
            }
            logger.LogError("Cancel Order was Faild!");
            return BadRequest(res.Message);
        }
    }
}
