using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Models.Requests;
using System;
using Microsoft.AspNetCore.SignalR;
using Smp.Web.Hubs;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class MessagesController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMessagesService _messagesService;
        private readonly IConversationsService _conversationsService;
        private readonly IHubContext<MessageHub> _messageHub;

        public MessagesController(IAuthService authService,
            IMessagesService messagesService,
            IConversationsService conversationsService,
            IHubContext<MessageHub> messageHub)
        {
            _authService = authService;
            _messagesService = messagesService;
            _conversationsService = conversationsService;
            _messageHub = messageHub;
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> CreateMessage([FromBody]CreateMessageRequest request)
        {
            var tkn = Request.Headers["Authorization"];
            if (!Guid.TryParse(_authService.GetUserIdFromToken(tkn), out var userId)
                || !(_authService.AuthorizeSelf(tkn, request.SenderId))
                || !await InConversation(userId, request.ConversationId))
                return Unauthorized();

            await _messagesService.CreateMessage(new Message(request));
            await _messageHub.Clients.All.SendAsync("newmessage", request.ConversationId);

            return Ok();
        }

        [HttpGet("[action]/{conversationId:Guid}"), Authorize]
        public async Task<IActionResult> GetMessagesFromConversation([FromRoute]Guid conversationId, [FromQuery]int count = 10, [FromQuery]int page = 0)
        {
            if (!Guid.TryParse(_authService.GetUserIdFromToken(Request.Headers["Authorization"]), out var userId)
                || !await InConversation(userId, conversationId))
                return Unauthorized();

            return Ok(await _messagesService.GetMessagesFromConversation(conversationId, count, page));
        }

        private async Task<bool> InConversation(Guid userId, Guid conversationId)
        {
            var conversationParticipantIds = await _conversationsService.GetConversationParticipants(conversationId);

            foreach (var id in conversationParticipantIds)
            {
                if (id != userId) continue;
                return true;
            }

            return false;
        }
    }
}