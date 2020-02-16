using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Models.Requests;
using System;
using System.Linq;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMessagesService _messagesService;
        private readonly IConversationsService _conversationsService;

        public MessagesController(IAuthService authService, IMessagesService messagesService, IConversationsService conversationsService)
        {
            _authService = authService;
            _messagesService = messagesService;
            _conversationsService = conversationsService;
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> CreateMessage([FromBody]CreateMessageRequest request)
        {
            var tkn = Request.Headers["Authorization"];
            if (!(_authService.AuthorizeSelf(tkn, request.SenderId) && await _authService.AuthorizeFriend(tkn, request.ReceiverId))) 
            {
                return Unauthorized();
            }

            await _messagesService.CreateMessage(new Message(request));

            return Ok();
        }

        [HttpGet("[action]/{conversationId:Guid}"), Authorize]
        public async Task<IActionResult> GetMessagesFromConversation([FromRoute]Guid conversationId, [FromQuery]int count = 10, [FromQuery]int page = 0)
        {
            if (!Guid.TryParse(_authService.GetUserIdFromToken(Request.Headers["Authorization"]), out var userId)) return Unauthorized();

            var conversationParticipantIds = await _conversationsService.GetConversationParticipants(conversationId);

            var inConversation = false;

            foreach (var id in conversationParticipantIds)
            {
                if (id != userId) continue;
                inConversation = true;
                break;
            }

            if (!inConversation) return Unauthorized();

            return Ok(await _messagesService.GetMessagesFromConversation(conversationId, count, page));
        }
    }
}