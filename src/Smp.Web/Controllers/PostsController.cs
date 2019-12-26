using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;
using Smp.Web.Models;
using Smp.Web.Services;
using Smp.Web.Validators;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest createPostRequest)
        {
            await _postsService.CreatePost(new Post(createPostRequest));

            return Ok();
        }

        [HttpGet("[action]/{userId:Guid}"), Authorize]
        public async Task<IActionResult> GetPosts(Guid userId, [FromQuery]int count = 10)
        {
            return Ok(await _postsService.GetPostsByReceiverId(userId, count));
        }
    }
}