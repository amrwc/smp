using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, ValidPassword)
                    .With(request => request.ConfirmPassword, ValidPassword)
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, ValidPassword)
                    .With(request => request.ConfirmPassword, ValidPassword)
                    .Without(request => request.FullName).Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, ValidPassword)
                    .With(request => request.ConfirmPassword, ValidPassword)
                    .With(request => request.FullName, "ye")
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .Without(request => request.Password)
                    .Without(request => request.ConfirmPassword)
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, "123")
                    .With(request => request.ConfirmPassword, "123")
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, "bobbobbobBob")
                    .With(request => request.ConfirmPassword, "bobbobbobBob")
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .Without(request => request.Email)
                    .With(request => request.Password, ValidPassword)
                    .With(request => request.ConfirmPassword, ValidPassword)
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
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
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                        .With(request => request.Email, "notanemail")
                        .With(request => request.Password, ValidPassword)
                        .With(request => request.ConfirmPassword, ValidPassword)
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_email", "Email must be a valid email address."));
        }

        [TestFixture]
        public class GivenATakenEmail : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;

            private IList<Error> _errors;

            [OneTimeSetUp]
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, ValidPassword)
                    .With(request => request.ConfirmPassword, ValidPassword)
                    .Create();

                UsersRepository.Setup(repository => repository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_email", "Email address is already in use. Please try another one."));
        }

        public class GivenDifferentPasswords : UserValidatorTestBase
        {
            private CreateUserRequest _createUserRequest;
            private IList<Error> _errors;

            [OneTimeSetUp]
            public async Task WhenValidateCreateUserRequestIsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _createUserRequest = fixture.Build<CreateUserRequest>()
                    .With(request => request.Email, ValidEmail)
                    .With(request => request.Password, ValidPassword)
                    .With(request => request.ConfirmPassword, "SomethingElse")
                    .Create();

                _errors = await UserValidator.ValidateCreateUserRequest(_createUserRequest);
            }

            [Test]
            public void ThenThereShouldBeAnError()
                => Assert.That(_errors.Count, Is.EqualTo(1));

            [Test]
            public void ThenTheErrorShouldBeAsExpected()
                => _errors.First().Should().BeEquivalentTo(new Error("invalid_password", "Passwords must match."));
        }
    }
}
