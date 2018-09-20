using Smp.Web.Services;
using Moq;
using Smp.Web.Repositories;
using Microsoft.Extensions.Configuration;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AuthServiceTests
{
    public class AuthServiceTestBase
    {
        protected Mock<IConfiguration> Configuration;
        protected Mock<IUserRepository> UserRepository;
        protected Mock<ICryptographyService> CryptographyService;
        protected IAuthService AuthService;

        public void Setup()
        {
            Configuration = new Mock<IConfiguration>();
            UserRepository = new Mock<IUserRepository>();
            CryptographyService = new Mock<ICryptographyService>();

            AuthService = new AuthService(Configuration.Object, UserRepository.Object, CryptographyService.Object);
        }
    }
}