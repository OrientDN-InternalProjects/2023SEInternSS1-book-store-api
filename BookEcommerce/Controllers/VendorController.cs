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
        public VendorController()
        {

        }

        [HttpGet("Authorize")]
        public async Task<IActionResult> Authorize()
        {
            return Ok(new
            {
                Role = "Vendor"
            });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProfile()
        {
            
        }
    }
}
