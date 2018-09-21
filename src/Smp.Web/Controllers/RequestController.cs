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
        private readonly IRequestRepository _requestRepository;

        public RequestController(IRequestService requestService, IRequestRepository requestRepository)
        {
            _requestService = requestService;
            _requestRepository = requestRepository;

        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> SendRequest([FromBody] RequestRequest requestRequest)
        {
            var validationResult = _requestService.ValidateRequest(new Request(requestRequest));
            if (validationResult.Any()) return BadRequest(validationResult);

            var newRequest = new Request(requestRequest);

            await _requestRepository.CreateRequest(newRequest);

            return Ok();
        }
    }
}