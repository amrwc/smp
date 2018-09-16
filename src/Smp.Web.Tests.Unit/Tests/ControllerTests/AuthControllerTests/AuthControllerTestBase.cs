using Moq;
using Smp.Web.Controllers;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AuthControllerTests
{
    public class AuthControllerTestBase
    {
        protected Mock<IAuthService> AuthService;
        protected AuthController AuthController;

        public void Setup()
        {
            AuthService = new Mock<IAuthService>();

            AuthController = new AuthController(AuthService.Object);
        }
    }
}
