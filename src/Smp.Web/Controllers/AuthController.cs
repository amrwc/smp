using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models.Requests;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("[action]")]
        public IActionResult SignIn([FromBody]LoginRequest loginRequest)
        {
            if (loginRequest.Email == "Bob@mail.com" && loginRequest.Password == "Bob'sPassword123")
            {
                return Ok(new { });
            }

            throw new System.NotImplementedException();
        }
    }
}