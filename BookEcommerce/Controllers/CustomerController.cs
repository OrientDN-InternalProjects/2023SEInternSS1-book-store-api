using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> CreateProfile([FromBody] CustomerDTO CustomerDTO)
        {
            var result = await this.customerService.CreateCustomer(CustomerDTO);
            if (result.IsSuccess) return StatusCode(StatusCodes.Status200OK, new ResponseBase
            {
                    IsSuccess = true,
                    Message = "OK"
            });

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseBase
            {
                    IsSuccess = false,
                    Message = "Bad request"
            });
        }
    }
}
