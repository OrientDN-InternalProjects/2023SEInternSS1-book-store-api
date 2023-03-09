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
    public class VendorController : ControllerBase
    {
        private readonly IVendorService vendorService;
        public VendorController(IVendorService vendorService)
        {
            this.vendorService = vendorService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "VENDOR")]
        [HttpPost("/create/vendor")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorCreateViewModel CreateVendorDTO)
        {
            string authHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            Console.WriteLine(authHeader);
            var result = await this.vendorService.CreateVendor(CreateVendorDTO, authHeader);
            if (result.IsSuccess)
                return Ok(new ResponseBase
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message
                });

            return BadRequest(new ResponseBase
            {
                IsSuccess = false,
                Message = "Create vendor failed"
            });
        }
    }
}
