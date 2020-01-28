using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Models.Requests;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMessagesRepository _messagesRepository;

        public MessagesController(IAuthService authService, IMessagesRepository messagesRepository)
        {
            _authService = authService;
            _messagesRepository = messagesRepository;
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> CreateMessage([FromBody]CreateMessageRequest request)
        {
            var tkn = Request.Headers["Authorization"];
            if (!(_authService.AuthorizeSelf(tkn, request.SenderId) && await _authService.AuthorizeFriend(tkn, request.ReceiverId))) 
            {
                return Unauthorized();
            }

            await _messagesRepository.CreateMessage(new Message(request));

            return Ok();
        }
    }
}