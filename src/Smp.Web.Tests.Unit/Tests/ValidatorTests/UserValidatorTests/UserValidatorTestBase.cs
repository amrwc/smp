using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.UserValidatorTests
{
    public class UserValidatorTestBase
    {
        protected UserValidator UserValidator;

        public void Setup()
        {
            UserValidator = new UserValidator();
        }
    }
}