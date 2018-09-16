using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models.Requests;
using Smp.Web.Services;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody]SignInRequest signInRequest)
        {
            var verifyUserResult = await _authService.VerifyUser(signInRequest.Email, signInRequest.Password);
            if (verifyUserResult.Success)
                return Ok(new { token = _authService.CreateJwt(verifyUserResult.User) });

            return verifyUserResult.Success
                ? (IActionResult) Ok(new { token = _authService.CreateJwt(verifyUserResult.User) })
                : Unauthorized();
        }
    }
}