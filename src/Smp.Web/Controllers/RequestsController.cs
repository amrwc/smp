using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Smp.Web.Models.Requests;

namespace Smp.Web.Controllers
{
    public class RequestsController : Controller
    {
        private readonly IRequestsService _requestsService;
        private readonly IRequestsRepository _requestsRepository;

        public RequestsController(IRequestsService requestsService, IRequestsRepository requestsRepository)
        {
            _requestsService = requestsService;
            _requestsRepository = requestsRepository;

        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetRequests(Guid userId)
            => Ok(await _requestsRepository.GetRequestsBySenderId(userId));

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> SendRequest([FromBody] RequestRequest requestRequest)
        {
            var newRequest = new Request(requestRequest);

            var validationResult = await _requestsService.ValidateNewRequest(newRequest);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestsRepository.CreateRequest(newRequest);

            return Ok();
        }

        [HttpGet("[action]/{userId:Guid}/{senderId:Guid}/{requestType:int}"), Authorize]
        public async Task<IActionResult> AcceptRequest(Guid userId, Guid senderId, byte requestTypeId)
        {
            var request = new Request
            {
                SenderId = senderId,
                ReceiverId = userId,
                RequestTypeId = requestTypeId
            };

            var validationResult = await _requestsService.ValidateAcceptRequest(request);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestsService.AcceptRequest(request);

            return Ok();
        }
    }
}