using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Smp.Web.Repositories;

namespace Smp.Web.Tests.Unit.RepositoryTests.UserRepositoryTests
{
    public class CreateUserTests
    {
        public class GivenValidInformation
        {
            private Mock<IConfiguration> _configuration;

            private UserRepository _userRepository;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                _configuration = new Mock<IConfiguration>();

                // _configuration.Setup(configuration => configuration.GetValue<string>(It.IsAny<string>()))
                //     .Returns("Hello");

                _userRepository = new UserRepository(_configuration.Object);
            }

            [Test]
            public void Test1()
            {

            }
        }
    }
}
