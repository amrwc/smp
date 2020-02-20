using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using Smp.Web.Models;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConversationsService _conversationsService;

        public ConversationsController(IAuthService authService, IConversationsService conversationsService)
        {
            _authService = authService;
            _conversationsService = conversationsService;
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

        [HttpGet("[action]"), Authorize]
        public async Task<IActionResult> CreateConversation(Message message)
        {
            if (!Guid.TryParse(_authService.GetUserIdFromToken(Request.Headers["Authorization"]), out var userId))
            {
                return Unauthorized();
            }

            // var conversationId = _conversationsService.
            // return Ok(conversationId);
            return null;
        }
    }
}