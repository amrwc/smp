using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Services;
using System;
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

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetRequests(Guid userId)
            => Ok(await _requestRepository.GetRequestsBySenderId(userId));

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> SendRequest([FromBody] RequestRequest requestRequest)
        {
            var newRequest = new Request(requestRequest);

            var validationResult = await _requestService.ValidateNewRequest(newRequest);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestRepository.CreateRequest(newRequest);

            return Ok();
        }

        [HttpGet("[action]/{userId:Guid}/{senderId:Guid}/{requestType:int}"), Authorize]
        public async Task<IActionResult> AcceptRequest(Guid userId, Guid senderId, int requestType)
        {
            var request = new Request
            {
                SenderId = senderId,
                ReceiverId = userId,
                RequestType = (RequestType) requestType
            };

            var validationResult = await _requestService.ValidateAcceptRequest(request);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestService.AcceptRequest(request);

            return Ok();
        }
    }
}