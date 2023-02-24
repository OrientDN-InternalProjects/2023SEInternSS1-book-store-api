using BookEcommerce.Models.DTOs;
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
        private readonly IVerifyAccountService? verifyAccountService;
        
        public AuthController(IAuthenticationService authenticationService, IVerifyAccountService verifyAccountService)
        {
            this.authenticationService = authenticationService;
            this.verifyAccountService = verifyAccountService;
        }
        [HttpPost("register/customer")]
        public async Task<IActionResult> CustomerRegister([FromForm] AccountDTO AccountDTO)
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
        [HttpPost("verify")]
        public async Task<IActionResult> Login([FromForm] LoginDTO LoginDTO)
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
        [HttpPost("register/vendor")]
        public async Task<IActionResult> VendorRegister([FromForm] AccountDTO AccountDTO)
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
        [HttpPost("create/admin")]
        public async Task<IActionResult> AdminRegister([FromForm] AccountDTO AccountDTO)
        {
            var result = await this.authenticationService!.AdminRegister(AccountDTO);
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
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromQuery] string Email, [FromBody] TokenDTO TokenDTO)
        {
            try
            {
                await this.authenticationService!.RefreshToken(Email, TokenDTO);
                return StatusCode(StatusCodes.Status200OK, new ResponseBase
                {
                    IsSuccess = true,
                    Message = "OK"
                });
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
