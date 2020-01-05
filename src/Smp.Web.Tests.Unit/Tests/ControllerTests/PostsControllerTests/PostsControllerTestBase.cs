using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Smp.Web.Controllers;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.PostsControllerTests
{
    public class PostsControllerTestBase
    {
        protected Mock<IPostsService> PostsService { get; set; }
        protected Mock<IAuthService> AuthService { get; set; }

        protected PostsController PostsController { get; set; }

        protected void Setup()
        {
            PostsService = new Mock<IPostsService>();
            AuthService = new Mock<IAuthService>();

            PostsController = new PostsController(PostsService.Object, AuthService.Object);
            PostsController.ControllerContext = new ControllerContext();
            PostsController.ControllerContext.HttpContext = new DefaultHttpContext();
            PostsController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCb2IgSmVua2lucyIsImp0aSI6IjAzZTE2MGMyLWZhNWItNDg0NS1hMjMwLTU5MDZlZTU1NWY1ZSIsImV4cCI6MTg5MzcwMTYyNywiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.C9_Y29A0ky2tObFpp7vyvm3vjlxSU4Tmfj_B1Mvyfh4");
        }
    }
}
