using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AccountsControllerTests
{
    public class ForgottenPasswordTests
    {
        protected const string Email = "email@email.com";

        public class GivenAnUnlinkedEmail : AccountsControllerTestBase
        {
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenForgottenPasswordGetsCalled()
            {
                Setup();

                UsersRepository.Setup(repository => repository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((User)null);

                _result = await AccountsController.ForgottenPassword(Email);
            }

            private NotFoundObjectResult NotFoundObjectResult
                => (NotFoundObjectResult) _result;

            [Test]
            public void ThenNotFoundShouldHaveBeenReturned()
                => Assert.IsInstanceOf<NotFoundObjectResult>(_result);

            [Test]
            public void ResultShouldBeAsExpected()
                => NotFoundObjectResult.Value.Should().BeEquivalentTo(new Error("invalid_email",
                    "Email must be linked to an existing user account."));

            [Test]
            public void UsersRepositoryGetUserByEmailShouldHaveBeenCalled()
                => UsersRepository.Verify(repository => repository.GetUserByEmail(Email), Times.Once);

            [Test]
            public void AccountsServiceInitiatePasswordResetShouldNotHaveBeenCalled()
                => AccountsService.Verify(
                    service => service.InitiatePasswordReset(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }

        public class GivenALinkedEmail : AccountsControllerTestBase
        {
            private User _user;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenForgottenPasswordGetsCalled()
            {
                Setup();

                _user = new Fixture().Create<User>();

                UsersRepository.Setup(repository => repository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(_user);

                _result = await AccountsController.ForgottenPassword(Email);
            }

            [Test]
            public void ThenOkShouldHaveBeenReturned()
                => Assert.IsInstanceOf<OkResult>(_result);

            [Test]
            public void UsersRepositoryGetUserByEmailShouldHaveBeenCalled()
                => UsersRepository.Verify(repository => repository.GetUserByEmail(Email), Times.Once);

            [Test]
            public void AccountsServiceInitiatePasswordResetShouldHaveBeenCalled()
                => AccountsService.Verify(
                    service => service.InitiatePasswordReset(_user.Id, _user.Email), Times.Once);
        }
    }
}
