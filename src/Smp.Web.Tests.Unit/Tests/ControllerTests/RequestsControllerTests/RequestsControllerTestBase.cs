using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Smp.Web.Controllers;
using Smp.Web.Repositories;
using Smp.Web.Services;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    public class RequestsControllerTestBase
    {
        protected Mock<IRequestsRepository> RequestsRepository;
        protected Mock<IRequestsService> RequestsService;
        protected Mock<IAuthService> AuthService;
        protected Mock<IRequestValidator> RequestValidator;

        protected RequestsController RequestsController;

        public void Setup()
        {
            RequestsRepository = new Mock<IRequestsRepository>();
            RequestsService = new Mock<IRequestsService>();
            AuthService = new Mock<IAuthService>();
            RequestValidator = new Mock<IRequestValidator>();

            RequestsController = new RequestsController(RequestsService.Object, RequestsRepository.Object, AuthService.Object, RequestValidator.Object);
            RequestsController.ControllerContext = new ControllerContext();
            RequestsController.ControllerContext.HttpContext = new DefaultHttpContext();
            RequestsController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCb2IgSmVua2lucyIsImp0aSI6IjAzZTE2MGMyLWZhNWItNDg0NS1hMjMwLTU5MDZlZTU1NWY1ZSIsImV4cCI6MTg5MzcwMTYyNywiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.C9_Y29A0ky2tObFpp7vyvm3vjlxSU4Tmfj_B1Mvyfh4");
        }
    }
}
