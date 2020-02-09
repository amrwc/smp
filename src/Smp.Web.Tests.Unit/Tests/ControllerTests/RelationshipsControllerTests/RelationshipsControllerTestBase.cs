using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Smp.Web.Controllers;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RelationshipsControllerTests
{
    public class RelationshipsControllerTestBase
    {
        protected Mock<IAuthService> AuthService { get; set; }
        protected Mock<IRelationshipsRepository> RelationshipsRepository { get; set; }

        protected RelationshipsController RelationshipsController { get; set; }

        protected void Setup()
        {
            AuthService = new Mock<IAuthService>();
            RelationshipsRepository = new Mock<IRelationshipsRepository>();

            RelationshipsController = new RelationshipsController(AuthService.Object, RelationshipsRepository.Object);
            RelationshipsController.ControllerContext = new ControllerContext();
            RelationshipsController.ControllerContext.HttpContext = new DefaultHttpContext();
            RelationshipsController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCb2IgSmVua2lucyIsImp0aSI6IjAzZTE2MGMyLWZhNWItNDg0NS1hMjMwLTU5MDZlZTU1NWY1ZSIsImV4cCI6MTg5MzcwMTYyNywiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.C9_Y29A0ky2tObFpp7vyvm3vjlxSU4Tmfj_B1Mvyfh4");
        }
    }
}
