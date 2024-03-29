﻿using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(Roles = "VENDOR", AuthenticationSchemes = "Bearer")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService vendorService;
        public VendorController(IVendorService vendorService)
        {
            this.vendorService = vendorService;
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "VENDOR")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateVendor([FromForm] VendorCreateViewModel CreateVendorDTO)
        {
            string AuthHeader = Request.Headers["Authorization"].ToString().Split(' ')[1];
            Console.WriteLine(AuthHeader);
            var result = await this.vendorService.CreateVendor(CreateVendorDTO, AuthHeader);
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
