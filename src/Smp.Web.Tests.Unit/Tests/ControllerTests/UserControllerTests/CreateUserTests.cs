using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.UserControllerTests
{
    [TestFixture]
    public class CreateUserTests
    {
        [TestFixture]
        public class GivenACreateUserRequestToFailValidation : UserControllerTestBase
        {
            private IActionResult _result;

            [OneTimeSetUp]
            public void WhenCreateUserHasBeenCalled()
            {
                Setup();

                UserValidator.Setup(validator => validator.ValidateCreateUserRequest(It.IsAny<CreateUserRequest>()))
                    .Returns(new List<Error> { new Error("idc", "idc") });

                _result = UserController.CreateUser(new CreateUserRequest { Username = "", Password = "bb", Email = "lx"});
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldNotHaveBeenCalled()
                => UserRepository.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Never);

            [Test]
            public void ThenABadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestObjectResult>(_result);
        }

        [TestFixture]
        public class GivenAValidCreateUserRequest : UserControllerTestBase
        {
            private IActionResult _result;

            [OneTimeSetUp]
            public void WhenCreateUserHasBeenCalled()
            {
                Setup();

                UserValidator.Setup(validator => validator.ValidateCreateUserRequest(It.IsAny<CreateUserRequest>()))
                    .Returns(new List<Error>());

                _result = UserController.CreateUser(new CreateUserRequest { Username = "asdasd", Password = "asdasd", Email = "asdasd"});
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldHaveBeenCalled()
                => UserRepository.Verify(repo => repo.CreateUser(It.Is<User>(user =>
                    user.Username == "asdasd" && user.Password == "asdasd" && user.Email == "asdasd")));

            [Test]
            public void ThenResultShouldBeAnOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }
    }
}
