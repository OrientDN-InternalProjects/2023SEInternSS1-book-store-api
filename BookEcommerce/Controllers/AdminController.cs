using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(Roles = "ADMIN", AuthenticationSchemes = "Bearer")]
    public class AdminController : ControllerBase
    {
        public AdminController()
        {

        }

        [HttpGet("/authorize")]
        public async Task<IActionResult> Authorize()
        {
            return Ok(new
            {
                role = "Admin"
            });
        }
    }
}
