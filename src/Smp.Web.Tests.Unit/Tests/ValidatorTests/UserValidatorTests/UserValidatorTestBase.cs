using Moq;
using Smp.Web.Repositories;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.UserValidatorTests
{
    public class UserValidatorTestBase
    {
        protected const string ValidFullName = "John Doe";
        protected const string ValidEmail = "valid@email.com";
        protected const string ValidPassword = "ValidPassword1!";

        protected Mock<IUsersRepository> UsersRepository { get; set; }

        protected UserValidator UserValidator;

        public void Setup()
        {
            UsersRepository = new Mock<IUsersRepository>();

            UserValidator = new UserValidator(UsersRepository.Object);
        }
    }
}