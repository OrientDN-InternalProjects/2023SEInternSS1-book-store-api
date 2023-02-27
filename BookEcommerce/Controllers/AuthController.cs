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
        public async Task<IActionResult> CustomerRegister([FromForm] AccountViewModel AccountDTO)
        {
            var result = await this.authenticationService!.CustomerRegister(AccountDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Bad Request"
                });
            }
            return Ok(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }
        [HttpPost("verify")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel LoginDTO)
        {
            var result = await this.authenticationService!.Login(LoginDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Failed to verify account"
                });
            }
            return Ok(new TokenResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Token = result.Token
            });
        }
        [HttpPost("register/vendor")]
        public async Task<IActionResult> VendorRegister([FromForm] AccountViewModel AccountDTO)
        {
            var result = await this.authenticationService!.VendorRegister(AccountDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Bad Request"
                });
            }
            return Ok(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }
        [HttpPost("create/admin")]
        public async Task<IActionResult> AdminRegister([FromForm] AccountViewModel AccountDTO)
        {
            var result = await this.authenticationService!.AdminRegister(AccountDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Bad Request"
                });
            }
            return Ok(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromQuery] string Email, [FromBody] TokenViewModel TokenDTO)
        {
            await this.authenticationService!.RefreshToken(Email, TokenDTO);
            return Ok(new ResponseBase
            {
                IsSuccess = true,
                Message = "Token refreshed"
            });
        }
    }
}

