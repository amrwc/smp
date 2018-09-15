using Smp.Web.Controllers;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AuthControllerTests
{
    public class AuthControllerTestBase
    {
        protected AuthController AuthController;

        public void Setup()
        {
            AuthController = new AuthController();
        }
    }
}
