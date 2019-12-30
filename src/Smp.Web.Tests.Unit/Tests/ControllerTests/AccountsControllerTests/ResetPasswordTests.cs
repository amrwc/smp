using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;
using Action = Smp.Web.Models.Action;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AccountsControllerTests
{
    public class ResetPasswordTests
    {
        public class GivenUnequalPasswords : AccountsControllerTestBase
        {
            private ResetPasswordRequest _resetPasswordRequest;
            private Action _action;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenCompletePasswordResetGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _action = fixture.Create<Action>();
                _resetPasswordRequest = fixture.Create<ResetPasswordRequest>();
                _resetPasswordRequest.ActionId = _action.Id;

                ActionsRepository.Setup(repository => repository.GetActionById(It.IsAny<Guid>())).ReturnsAsync(_action);
                ActionValidator.Setup(validator => validator.ValidateAction(It.IsAny<Action>(), It.IsAny<ActionType>()))
                    .Returns(new List<Error>());

                _result = await AccountsController.ResetPassword(_resetPasswordRequest);
            }

            private BadRequestObjectResult BadRequestObjectResult
                => (BadRequestObjectResult) _result;

            [Test]
            public void ThenBadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestObjectResult>(_result);

            [Test]
            public void ThereShouldBeOneError()
                => Assert.That(((IList<Error>) BadRequestObjectResult.Value).Count, Is.EqualTo(1));

            [Test]
            public void TheErrorShouldBeAsExpected()
                => ((IList<Error>)BadRequestObjectResult.Value).First().Should().BeEquivalentTo(new Error("invalid_password",
                    "Passwords must match."));

            [Test]
            public void ActionsRepositoryGetActionByIdShouldHaveBeenCalled()
                => ActionsRepository.Verify(repository => repository.GetActionById(_action.Id), Times.Once);

            [Test]
            public void ActionValidatorValidateActionShouldHaveBeenCalled()
                => ActionValidator.Verify(validator => validator.ValidateAction(_action, ActionType.ResetPassword), Times.Once);

            [Test]
            public void AccountsServiceCompletePasswordResetShouldNotHaveBeenCalled()
                => AccountsService.Verify(service => service.CompletePasswordReset(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);

            [Test]
            public void CryptographyServiceHashAndSaltPasswordShouldNotHaveBeenCalled()
                => CryptographyService.Verify(service => service.HashAndSaltPassword(It.IsAny<string>()), Times.Never);
        }

        public class GivenNoValidationErrors : AccountsControllerTestBase
        {
            private ResetPasswordRequest _resetPasswordRequest;
            private Action _action;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenCompletePasswordResetGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _action = fixture.Create<Action>();
                _resetPasswordRequest = fixture.Create<ResetPasswordRequest>();
                _resetPasswordRequest.ConfirmNewPassword = _resetPasswordRequest.NewPassword;
                _resetPasswordRequest.ActionId = _action.Id;

                ActionsRepository.Setup(repository => repository.GetActionById(It.IsAny<Guid>())).ReturnsAsync(_action);
                ActionValidator.Setup(validator => validator.ValidateAction(It.IsAny<Action>(), It.IsAny<ActionType>()))
                    .Returns(new List<Error>());
                CryptographyService.Setup(service => service.HashAndSaltPassword(It.IsAny<string>()))
                    .Returns("HashedAndSalted");

                _result = await AccountsController.ResetPassword(_resetPasswordRequest);
            }
            
            [Test]
            public void ThenOkShouldHaveBeenReturned()
                => Assert.IsInstanceOf<OkResult>(_result);
            
            [Test]
            public void ActionsRepositoryGetActionByIdShouldHaveBeenCalled()
                => ActionsRepository.Verify(repository => repository.GetActionById(_action.Id), Times.Once);

            [Test]
            public void ActionValidatorValidateActionShouldHaveBeenCalled()
                => ActionValidator.Verify(validator => validator.ValidateAction(_action, ActionType.ResetPassword), Times.Once);

            [Test]
            public void AccountsServiceCompletePasswordResetShouldHaveBeenCalled()
                => AccountsService.Verify(service => service.CompletePasswordReset(_action.UserId, "HashedAndSalted", _action.Id), Times.Once);

            [Test]
            public void CryptographyServiceHashAndSaltPasswordShouldHaveBeenCalled()
                => CryptographyService.Verify(service => service.HashAndSaltPassword(_resetPasswordRequest.NewPassword), Times.Once);
        }
    }
}
