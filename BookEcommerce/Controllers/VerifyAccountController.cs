﻿using BookEcommerce.Models.DTOs.Request;
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

        [HttpPost("/send-mail")]
        public async Task<IActionResult> SendMail([FromBody] SendMailRequest email)
        {
            var result = await this.verifyAccountService.SendVerificationMail(email.Email!);
            if(result.IsSuccess)
            {
                return Ok(new ResponseBase
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message
                });
            }
            return BadRequest(result.Message);
        }

        [HttpGet("/submit")]
        public async Task<IActionResult> ConfirmMail([FromQuery] string token, string email)
        {
            var result = await this.verifyAccountService.ConfirmMail(email);
            if (!result.IsSuccess) 
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Message = "submit failed"
                });
            return Ok(new ResponseBase
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }
    }
}
