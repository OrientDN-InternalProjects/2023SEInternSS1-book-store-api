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
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthenticationService authenticationService, IVerifyAccountService verifyAccountService, ILogger<AuthController> logger)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
            this.verifyAccountService = verifyAccountService;
        }

        [HttpPost("/register/customer")]
        public async Task<IActionResult> CustomerRegister([FromBody] AccountViewModel AccountDTO)
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

        [HttpPost("/verify")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel LoginDTO)
        {
            var result = await this.authenticationService!.Login(LoginDTO);
            if (!result.IsSuccess)
            {
                logger.LogError("Wrong credential");
                return Ok(new TokenResponse
                {
                    IsSuccess = result.IsSuccess,
                    IsActive = result.IsActive,
                    Message = result.Message
                });
            }
            logger.LogInformation("A user has logged to the system");
            return Ok(new TokenResponse
            {
                IsSuccess = result.IsSuccess,
                IsActive = result.IsActive,
                Message = result.Message,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("/register/vendor")]
        public async Task<IActionResult> VendorRegister([FromBody] AccountViewModel AccountDTO)
        {
            var result = await this.authenticationService!.VendorRegister(AccountDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(new ResponseBase
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

        [HttpPost("/create/admin")]
        public async Task<IActionResult> AdminRegister([FromBody] AccountViewModel AccountDTO)
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

        [HttpPost("/refresh")]
        public async Task<IActionResult> RefreshToken([FromQuery] string Email, [FromBody] TokenViewModel TokenDTO)
        {
            var result = await this.authenticationService!.RefreshToken(Email, TokenDTO);
            return Ok(new TokenResponse
            {
                IsSuccess = true,
                Message = "Token refreshed",
                AccessToken = result.AccessToken
            });
        }

        [HttpGet("/user")]
        public IActionResult getUser()
        {
            var authHeader = this.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = this.authenticationService!.GetUserLogged(authHeader);
            return Ok(new UserLoggedResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                UserName = result.UserName,
                UserId = result.UserId           
            });
        }
    }
}

