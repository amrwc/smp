using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ConversationsController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConversationsService _conversationsService;
        private readonly IMessagesService _messagesService;

        public ConversationsController(IAuthService authService, IConversationsService conversationsService, IMessagesService messagesService)
        {
            _authService = authService;
            _conversationsService = conversationsService;
            _messagesService = messagesService;
        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetConversations([FromRoute]Guid userId)
        {
            if (!_authService.AuthorizeSelf(Request.Headers["Authorization"], userId)) return Unauthorized();
            
            return Ok(await _conversationsService.GetConversations(userId));
        }

        [HttpGet("[action]/{conversationId:Guid}"), Authorize]
        public async Task<IActionResult> GetConversationParticipants([FromRoute]Guid conversationId)
        {
            if(!Guid.TryParse(_authService.GetUserIdFromToken(Request.Headers["Authorization"]), out var userId)) return Unauthorized();

            var conversationParticipantIds = await _conversationsService.GetConversationParticipants(conversationId);

            if (conversationParticipantIds.All(id => id != userId)) return Unauthorized();

            return Ok(conversationParticipantIds);
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> CreateConversation(CreateConversationRequest createConversationRequest)
        {
            if (!_authService.AuthorizeSelf(Request.Headers["Authorization"], createConversationRequest.SenderId)) return Unauthorized();

            var conversationId = await _conversationsService.CreateConversationWithParticipants(
                createConversationRequest.SenderId,
                createConversationRequest.ReceiverId);

            if (!string.IsNullOrEmpty(createConversationRequest.Content))
                await _messagesService.CreateMessage(new Message(createConversationRequest, conversationId));
            
            return Ok(conversationId);
        }
    }
}