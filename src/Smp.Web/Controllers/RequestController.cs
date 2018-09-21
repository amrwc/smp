using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Smp.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IUserRepository _userRepository;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> SendRequest([FromBody] RequestRequest requestRequest)
        {
            var validationResult = _requestService.ValidateRequest(new Request(requestRequest));
            if (validationResult.Any()) return BadRequest(validationResult);

            var newRequest = new Request(requestRequest);

            // _requestRepository.CreateRequest(newRequest);

            return Ok();
        }
    }
}