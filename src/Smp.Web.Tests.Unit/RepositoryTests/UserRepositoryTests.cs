using Microsoft.Extensions.Configuration;
using Moq;
using Smp.Web.Repositories;
using Xunit;

namespace Smp.Web.Tests.Unit.RepositoryTests
{
    public class UserRepositoryTests
    {
        public class GivenAConfiguration
        {
            private readonly Mock<IConfiguration> _configuration;

            private UserRepository _userRepository;

            public GivenAConfiguration()
            {
                _configuration = new Mock<IConfiguration>();

                _configuration.Setup(configuration => configuration.GetValue<string>(It.IsAny<string>()))
                    .Returns("Hello");

                _userRepository = new UserRepository(_configuration.Object);
            }

            [Fact]
            public void Test1()
            {

            }
        }
    }
}
