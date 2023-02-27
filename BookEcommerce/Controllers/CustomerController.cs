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
        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromForm] CustomerViewModel CustomerDTO)
        {
            string AuthHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            Console.WriteLine(AuthHeader);
            var result = await this.customerService.CreateCustomer(CustomerDTO, AuthHeader);

            if (result.IsSuccess)
                return Ok(new ResponseBase
                {
                    IsSuccess = true,
                    Message = result.Message
                });

            return BadRequest(new ResponseBase
            {
                IsSuccess = false,
                Message = "create customer failed"
            });
        }
    }
}
