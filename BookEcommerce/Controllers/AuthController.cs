using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService? authenticationService;
        
        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CustomerRegister([FromBody] AccountDTO AccountDTO)
        {
            var result = await this.authenticationService!.CustomerRegister(AccountDTO);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Bad Request"
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseBase
            {
                IsSuccess = true,
                Message = "OK"
            });
        }
        [HttpPost("Verify")]
        public async Task<IActionResult> Login([FromBody] LoginDTO LoginDTO)
        {
            var result = await this.authenticationService!.Login(LoginDTO);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Bad Request"
                });
            }
            return StatusCode(StatusCodes.Status200OK, new TokenResponse
            {
                IsSuccess = true,
                Message = "OK",
                Token = result.Token
            });
        }
        [HttpPost("CreateVendor")]
        public async Task<IActionResult> VendorRegister([FromBody] AccountDTO AccountDTO)
        {
            var result = await this.authenticationService!.VendorRegister(AccountDTO);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Bad Request"
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseBase
            {
                IsSuccess = true,
                Message = "OK"
            });
        }
    }
}
