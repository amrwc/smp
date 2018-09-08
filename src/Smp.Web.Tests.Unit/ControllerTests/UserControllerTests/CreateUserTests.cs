using Moq;
using NUnit.Framework;
using Smp.Web.Repositories;
using Smp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Smp.Web.Tests.Unit.ControllerTests.UserControllerTests
{
    [TestFixture]
    public class CreateUserTests
    {
        [TestFixture]
        public class GivenAnEmptyUsername : UserControllerTestBase
        {
            IActionResult result;

            [OneTimeSetUp]
            public void WhenCreateUserHasBeenCalled()
            {
                Setup();

                result = UserController.CreateUser("", "bob", "ccc");
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldNotHaveBeenCalled()
                => UserRepository.Verify(repo => repo.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            [Test]
            public void ThenABadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestFixture]
        public class GivenEmptyPassword : UserControllerTestBase
        {
            IActionResult result;

            [OneTimeSetUp]
            public void WhenCreateUserHasBeenCalled()
            {
                Setup();

                result = UserController.CreateUser("lll", "", "ccc");
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldNotHaveBeenCalled()
                => UserRepository.Verify(repo => repo.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            [Test]
            public void ThenABadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestFixture]
        public class GivenAnEmptyEmail : UserControllerTestBase
        {
            IActionResult result;

            [OneTimeSetUp]
            public void WhenCreateUserHasBeenCalled()
            {
                Setup();

                result = UserController.CreateUser("kajnf", "bob", "");
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldNotHaveBeenCalled()
                => UserRepository.Verify(repo => repo.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            [Test]
            public void ThenABadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestFixture]
        public class GivenValidInformation : UserControllerTestBase
        {
            private IActionResult _result;

            [OneTimeSetUp]
            public void WhenCreateUserHasBeenCalled()
            {
                Setup();

                _result = UserController.CreateUser("Hell", "O", "P");
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldHaveBeenCalled()
                => UserRepository.Verify(repo => repo.CreateUser("Hell", "O", "P"));

            [Test]
            public void ThenResultShouldBeAnOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }
    }
}
