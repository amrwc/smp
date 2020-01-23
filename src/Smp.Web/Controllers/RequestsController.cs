using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Smp.Web.Models.Requests;
using Smp.Web.Validators;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class RequestsController : Controller
    {
        private readonly IRequestsService _requestsService;
        private readonly IRequestsRepository _requestsRepository;
        private readonly IAuthService _authService;
        private readonly IRequestValidator _requestValidator;

        public RequestsController(IRequestsService requestsService, IRequestsRepository requestsRepository, IAuthService authService, IRequestValidator requestValidator)
        {
            _requestsService = requestsService;
            _requestsRepository = requestsRepository;
            _authService = authService;
            _requestValidator = requestValidator;
        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetOutgoingRequests([FromRoute]Guid userId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, userId)) return Unauthorized();

            return Ok(await _requestsRepository.GetRequestsBySenderId(userId));
        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetIncomingRequests([FromRoute]Guid userId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, userId)) return Unauthorized();

            return Ok(await _requestsRepository.GetRequestsByReceiverId(userId));
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> SendRequest([FromBody]RequestRequest requestRequest)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, requestRequest.SenderId)) return Unauthorized();

            var newRequest = new Request(requestRequest);

            var validationResult = await _requestValidator.ValidateNewRequest(newRequest);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestsRepository.CreateRequest(newRequest);

            return Ok();
        }

        [HttpGet("[action]/{receiverId:Guid}/{requestTypeId:int}/{senderId:Guid}/"), Authorize]
        public async Task<IActionResult> AcceptRequest([FromRoute]Guid receiverId, [FromRoute]Guid senderId, [FromRoute]byte requestTypeId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, receiverId)) return Unauthorized();

            var request = new Request
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                RequestType = (RequestType)requestTypeId
            };

            var validationResult = await _requestValidator.ValidateAcceptRequest(request);
            if (validationResult.Any()) return BadRequest(validationResult);

            await _requestsService.AcceptRequest(request);

            return Ok();
        }

        [HttpGet("[action]/{receiverId:Guid}/{requestTypeId:int}/{senderId:Guid}/"), Authorize]
        public async Task<IActionResult> DeclineRequest(Guid receiverId, Guid senderId, byte requestTypeId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, receiverId)) return Unauthorized();

            var request = await _requestsRepository.GetRequestByUserIdsAndType(receiverId, senderId, (RequestType) requestTypeId);
            if (request == null) return BadRequest(new Error("invalid_request", "There is no pending request."));

            await _requestsService.DeclineRequest(request);

            return Ok();
        }

        [HttpGet("[action]/{receiverId:Guid}/{requestTypeId:int}/{senderId:Guid}/"), Authorize]
        public async Task<IActionResult> GetRequest([FromRoute]Guid receiverId, [FromRoute]Guid senderId, [FromRoute]byte requestTypeId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!(_authService.AuthorizeSelf(tkn, receiverId) || _authService.AuthorizeSelf(tkn, senderId))) return Unauthorized();

            var expectedReq = new Request { ReceiverId = receiverId, SenderId = senderId, RequestType = (RequestType)requestTypeId };
            var req = await _requestsRepository.GetRequestByUserIdsAndType(expectedReq);

            return req == null ? (IActionResult) NotFound() : Ok(req);
        }
    }
}