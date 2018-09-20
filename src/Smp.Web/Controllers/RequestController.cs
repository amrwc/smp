using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models;
using Smp.Web.Services;

namespace Smp.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("[action]"), Authorize]
        public IActionResult SendRequest([FromBody] RequestRequest requestRequest)
        {
            return Ok();
        }
    }
}