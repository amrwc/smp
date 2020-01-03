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
    [Route("api/[controller]")]
    public class RequestsController : Controller
    {
        private readonly IRequestsService _requestsService;
        private readonly IRequestsRepository _requestsRepository;
        private readonly IAuthService _authService;

        public RequestsController(IRequestsService requestsService, IRequestsRepository requestsRepository, IAuthService authService)
        {
            _requestsService = requestsService;
            _requestsRepository = requestsRepository;
            _authService = authService;
        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetOutgoingRequests(Guid userId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, userId)) return Unauthorized();

            return Ok(await _requestsRepository.GetRequestsBySenderId(userId));
        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetIncomingRequests(Guid userId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, userId)) return Unauthorized();

            return Ok(await _requestsRepository.GetRequestsByReceiverId(userId));
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> SendRequest([FromBody] RequestRequest requestRequest)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, requestRequest.SenderId)) return Unauthorized();

            var newRequest = new Request(requestRequest);

            var validationResult = await _requestsService.ValidateNewRequest(newRequest);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestsRepository.CreateRequest(newRequest);

            return Ok();
        }

        [HttpGet("[action]/{userId:Guid}/{senderId:Guid}/{requestType:int}"), Authorize]
        public async Task<IActionResult> AcceptRequest(Guid userId, Guid senderId, byte requestTypeId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, userId)) return Unauthorized();

            var request = new Request
            {
                SenderId = senderId,
                ReceiverId = userId,
                RequestType = (RequestType)requestTypeId
            };

            var validationResult = await _requestsService.ValidateAcceptRequest(request);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestsService.AcceptRequest(request);

            return Ok();
        }
    }
}