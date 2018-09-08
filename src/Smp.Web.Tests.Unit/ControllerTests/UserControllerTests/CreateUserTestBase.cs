using Moq;
using Smp.Web.Repositories;
using Smp.Web.Controllers;

namespace Smp.Web.Tests.Unit.ControllerTests.UserControllerTests
{
    public class UserControllerTestBase
    {
        protected Mock<IUserRepository> UserRepository;

        protected UserController UserController;

        public void Setup()
        {
            UserRepository = new Mock<IUserRepository>();

            UserController = new UserController(UserRepository.Object);
        }
    }
}