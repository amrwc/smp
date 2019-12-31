using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AccountsServiceTests
{
    public class CompletePasswordResetTests : AccountsServiceTestBase
    {
        public class GivenValidArguments : AccountsServiceTestBase
        {
            private const string Password = "Password";
            private readonly Guid _userId = Guid.NewGuid();
            private readonly Guid _actionId = Guid.NewGuid();

            [OneTimeSetUp]
            public async Task WhenCompletePasswordResetGetsCalled()
            {
                Setup();

                await AccountService.CompletePasswordReset(_userId, Password, _actionId);
            }

            [Test]
            public void ThenUsersRepositoryByIdShouldHaveBeenCalled()
                => UsersRepository.Verify(repository => repository.UpdatePasswordById(_userId, Password), Times.Once);

            [Test]
            public void ThenActionsRepositoryCompleteActionByIdShouldHaveBeenCalled()
                => ActionsRepository.Verify(repository => repository.CompleteActionById(_actionId), Times.Once);
        }
    }
}
