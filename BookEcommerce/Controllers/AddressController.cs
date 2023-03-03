using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService; 
        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "CUSTOMER")]
        [HttpPost("create-address")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressViewModel addressViewModel)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
                var result = await this.addressService.CreateAddress(addressViewModel, authHeader);
                return Ok(new ResponseBase
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message
                });
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Message = e.Message
                });
            }
        }
    }
}
