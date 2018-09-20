using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.UserValidatorTests
{
    [TestFixture]
    public class ValidateCreateUserRequestTests
    {
        [TestFixture]
        public class GivenAValidCreateUserRequest : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "email@email.com").Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenNoErrorShouldHaveBeenReturned()
                => Assert.IsEmpty(_errors);
        }

        [TestFixture]
        public class GivenAnEmptyFullName : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "email@email.com").Without(request => request.FullName).Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_full_name", "Full name must have at least 3 characters."));
        }

        public class GivenTooShortFullName : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "email@email.com").With(request => request.FullName, "ye").Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_full_name", "Full name must have at least 3 characters."));
        }

        [TestFixture]
        public class GivenAnEmptyPassword : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "email@email.com").Without(request => request.Password).Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_password", "Password must have at least 8 characters, at least 1 lowercase letter, at least 1 uppercase letter, a number, and a symbol."));
        }

        [TestFixture]
        public class GivenTooShortPassword : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "email@email.com").With(request => request.Password, "123").Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_password", "Password must have at least 8 characters, at least 1 lowercase letter, at least 1 uppercase letter, a number, and a symbol."));
        }

        [TestFixture]
        public class GivenInsecurePassword : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "email@email.com").With(request => request.Password, "bobbobbobBob").Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_password", "Password must have at least 8 characters, at least 1 lowercase letter, at least 1 uppercase letter, a number, and a symbol."));
        }

        [TestFixture]
        public class GivenAnEmptyEmail : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().Without(request => request.Email).Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_email", "Email must be a valid email address."));
        }

        [TestFixture]
        public class GivenAnInvalidEmail : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public void WhenCreateUserIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>().With(request => request.Email, "notanemail").Create();

                _errors = UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_email", "Email must be a valid email address."));
        }
    }
}
