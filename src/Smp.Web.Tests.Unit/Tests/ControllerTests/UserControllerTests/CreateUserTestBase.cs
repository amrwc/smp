using Moq;
using Smp.Web.Controllers;
using Smp.Web.Repositories;
using Smp.Web.Services;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.UserControllerTests
{
    public class UserControllerTestBase
    {
        protected Mock<IUserRepository> UserRepository;
        protected Mock<IUserValidator> UserValidator;
        protected Mock<ICryptographyService> CryptographyService;

        protected UserController UserController;

        public void Setup()
        {
            UserRepository = new Mock<IUserRepository>();
            UserValidator = new Mock<IUserValidator>();
            CryptographyService = new Mock<ICryptographyService>();

            UserController = new UserController(UserRepository.Object, UserValidator.Object, CryptographyService.Object);
        }
    }
}