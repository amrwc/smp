using Moq;
using Smp.Web.Repositories;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.UserValidatorTests
{
    public class UserValidatorTestBase
    {
        protected Mock<IUsersRepository> UsersRepository { get; set; }

        protected UserValidator UserValidator;

        public void Setup()
        {
            UsersRepository = new Mock<IUsersRepository>();

            UserValidator = new UserValidator(UsersRepository.Object);
        }
    }
}