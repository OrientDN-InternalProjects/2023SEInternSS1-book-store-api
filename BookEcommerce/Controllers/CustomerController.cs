using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]


    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "CUSTOMER")]
        [HttpPost("/create/customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerViewModel CustomerDTO)
        {       
            string authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var result = await this.customerService.CreateCustomer(CustomerDTO, authHeader);
            if (result.IsSuccess)
            {
                return Ok(new ResponseBase
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message
                });
            }
            return Ok(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "CUSTOMER")]
        [HttpGet("/get-customer")]
        public async Task<IActionResult> GetCustomerProfile()
        {
            string authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var result = await this.customerService.GetCustomerProfile(authHeader);
            if (result.IsSuccess)
            {
                return Ok(new CustomerResponse
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                    CustomerFullName = result.CustomerFullName,
                    CustomerId = result.CustomerId
                });
            }
            return Unauthorized(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }
    }
}
