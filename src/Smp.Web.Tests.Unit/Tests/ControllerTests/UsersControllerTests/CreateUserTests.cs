using System.Collections.Generic;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.UsersControllerTests
{
    [TestFixture]
    public class CreateUserTests
    {
        [TestFixture]
        public class GivenACreateUserRequestToFailValidation : UserControllerTestBase
        {
            private IActionResult _result;

            [OneTimeSetUp]
            public async void WhenCreateUserHasBeenCalled()
            {
                Setup();

                UserValidator.Setup(validator => validator.ValidateCreateUserRequest(It.IsAny<CreateUserRequest>()))
                    .Returns(new List<Error> { new Error("idc", "idc") });

                _result = await UsersController.CreateUser(new CreateUserRequest { FullName = "", Password = "bb", Email = "lx"});
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldNotHaveBeenCalled()
                => UsersRepository.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Never);

            [Test]
            public void ThenABadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestObjectResult>(_result);
        }

        [TestFixture]
        public class GivenAValidCreateUserRequest : UserControllerTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IActionResult _result;

            [OneTimeSetUp]
            public async void WhenCreateUserHasBeenCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Create<CreateUserRequest>();

                CryptographyService.Setup(service => service.HashAndSaltPassword(It.IsAny<string>())).Returns("HashedAndSaltedPassword");
                UserValidator.Setup(validator => validator.ValidateCreateUserRequest(It.IsAny<CreateUserRequest>()))
                    .Returns(new List<Error>());

                _result = await UsersController.CreateUser(_createUserRequest);
            }

            [Test]
            public void ThenUserRepositoryCreateUserShouldHaveBeenCalled()
                => UsersRepository.Verify(repo => repo.CreateUser(It.Is<User>(user =>
                    user.FullName == _createUserRequest.FullName && user.Password == "HashedAndSaltedPassword" &&
                    user.Email == _createUserRequest.Email)));

            [Test]
            public void ThenCryptographyServiceHashAndSaltPasswordShouldHaveBeenCalled()
                => CryptographyService.Verify(service => service.HashAndSaltPassword(_createUserRequest.Password), Times.Once);

            [Test]
            public void ThenResultShouldBeAnOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }
    }
}
