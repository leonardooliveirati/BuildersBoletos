using Microsoft.AspNetCore.Mvc;

namespace BuildersBoleto.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetToken()
        {
            var token = Request.Headers["Authorization"].ToString();
            return Ok(new { Token = token });
        }
    }
}
