using Smp.Web.Services;
using Moq;
using Smp.Web.Repositories;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AuthServiceTests
{
    public class AuthServiceTestBase
    {
        protected Mock<IUserRepository> UserRepository;
        protected IAuthService AuthService;

        public void Setup()
        {
            UserRepository = new Mock<IUserRepository>();

            AuthService = new AuthService();
        }
    }
}