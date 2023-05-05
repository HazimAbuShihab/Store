using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Core7.Repository;

namespace Store_Core7.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [ActionName("Test")]
        [Authorize(Roles = "Admin,User,Customer")]
        public IActionResult Test()
        {
            return Ok(new { Message = "API is running normally" });
        }
    }
}
