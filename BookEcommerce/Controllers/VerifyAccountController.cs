using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyAccountController : ControllerBase
    {
        private readonly IVerifyAccountService verifyAccountService;
        public VerifyAccountController(IVerifyAccountService verifyAccountService)
        {
            this.verifyAccountService = verifyAccountService;
        }
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail([FromForm] string Email)
        {
            await this.verifyAccountService.SendVerificationMail(Email);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase
            {
                IsSuccess = true,
                Message = "Sent"
            });
        }
        [HttpGet("submit")]
        public async Task<IActionResult> ConfirmMail([FromQuery] string token, string email)
        {
            var result = await this.verifyAccountService.ConfirmMail(email);
            if (!result.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new ResponseBase
            {
                IsSuccess = false,
                Message = "Bad Request"
            });
            return StatusCode(StatusCodes.Status200OK, new ResponseBase
            {
                IsSuccess = true,
                Message = "OK"
            });
        }
    }
}
