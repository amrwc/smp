using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Action = Smp.Web.Models.Action;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AccountsServiceTests
{
    public class InitiatePasswordResetTests
    {
        public class GivenValidArguments : AccountsServiceTestBase
        {
            private const string Email = "real@email.com";
            private readonly Guid _userId = Guid.NewGuid();

            private Action _expectedAction;
            private Action _usedAction;

            [OneTimeSetUp]
            public async Task WhenInitiatePasswordResetGetsCalled()
            {
                Setup();

                _expectedAction = new Action(_userId, ActionType.ResetPassword);

                ActionsRepository.Setup(repository => repository.CreateAction(It.IsAny<Action>()))
                    .Callback<Action>(action => _usedAction = action);

                await AccountService.InitiatePasswordReset(_userId, Email);
            }

            [Test]
            public void ThenActionsRepositoryCreateActionShouldHaveBeenCalled()
                => ActionsRepository.Verify(repository => repository.CreateAction(It.IsAny<Action>()), Times.Once);

            [Test]
            public void ThenUsedActionShouldBeAsExpected()
            {
                _usedAction.Should().BeEquivalentTo(_expectedAction,
                    options => options
                        .Excluding(action => action.Id)
                        .Excluding(action => action.CreatedAt)
                        .Excluding(action => action.ExpiresAt));
                _usedAction.CreatedAt.Should().BeCloseTo(_expectedAction.CreatedAt, 200);
                _usedAction.ExpiresAt.Should().BeCloseTo(_expectedAction.ExpiresAt, 200);
            }

            [Test]
            public void ThenMailServiceSendEmailShouldHaveBeenCalled()
                => MailService.Verify(
                    service => service.SendEmail(Email, "SMP - Password Reset", It.IsAny<string>()),
                    Times.Once);
        }
    }
}
