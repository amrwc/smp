using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.ActionValidatorTests
{
    public class ValidateActionTests
    {
        public class GivenANonExistentAction : ActionValidatorTestBase
        {
            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenValidateActionGetsCalled()
            {
                Setup();

                _errors = ActionValidator.ValidateAction(null, ActionType.ResetPassword);
            }

            [Test]
            public void ThenThereShouldBeOneError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_action", "Action must exist."));
        }

        public class GivenAnActionWithIncorrectTypeAndCompleteAndExpired : ActionValidatorTestBase
        {
            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenValidateActionGetsCalled()
            {
                Setup();

                var action = new Models.Action()
                {
                    ActionType = ActionType.None,
                    Completed = true,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
                };

                _errors = ActionValidator.ValidateAction(action, ActionType.ResetPassword);
            }

            [Test]
            public void ThenThereShouldBeThreeErrors()
                => Assert.That(_errors.Count, Is.EqualTo(3));

            [Test]
            public void ThenTheErrorsShouldBeAsExpected()
            {
                _errors[0].Should().BeEquivalentTo(new Error("invalid_action", "Action must be of correct type."));
                _errors[1].Should().BeEquivalentTo(new Error("invalid_action", "Action must not have been completed already."));
                _errors[2].Should().BeEquivalentTo(new Error("invalid_expiry", "Action must not have expired."));
            }
        }
    }
}
