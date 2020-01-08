using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.UsersControllerTests
{
    public class GetUserTests
    {
        public class GivenAnInvalidUserId : UserControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetUserGetsCalled()
            {
                Setup();

                _result = await UsersController.GetUser(_userId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<NotFoundResult>(_result);

            [Test]
            public void ThenUserRepositoryGetUserByIdShouldHaveBeenCalled()
                => UsersRepository.Verify(repository => repository.GetUserById(_userId), Times.Once);
        }

        public class GivenAValidRequest : UserControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();

            private IActionResult _result;

            private User _user;

            [OneTimeSetUp]
            public async Task WhenGetUserGetsCalled()
            {
                Setup();

                _user = new Fixture().Create<User>();
                _user.Password = string.Empty;

                UsersRepository.Setup(repository => repository.GetUserById(It.IsAny<Guid>()))
                    .ReturnsAsync(_user);

                _result = await UsersController.GetUser(_userId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenUserRepositoryGetUserByIdShouldHaveBeenCalled()
                => UsersRepository.Verify(repository => repository.GetUserById(_userId), Times.Once);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => ((OkObjectResult) _result).Value.Should().BeEquivalentTo(_user);
        }
    }
}
