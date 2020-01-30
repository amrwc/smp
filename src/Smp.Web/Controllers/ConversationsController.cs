using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Models.Requests;
using System;

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
            if (!(_authService.AuthorizeSelf(Request.Headers["Authorization"], userId))) return Unauthorized();
            
            return Ok(await _conversationsService.GetConversations(userId));
        }
    }
}