using NUnit.Framework;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.CryptographyServiceTests
{
    [TestFixture]
    public class HashAndSaltPasswordTests
    {
        [TestFixture]
        public class GivenAString : CryptographyServiceTestBase
        {
            private const string Password = "hello";
            private string _hashedAndSaltedPassword;

            private ICryptographyService _cryptographyService;

            [OneTimeSetUp]
            public void WhenHashAndSaltPasswordGetsCalled()
            {
                _cryptographyService = new CryptographyService();

                _hashedAndSaltedPassword = _cryptographyService.HashAndSaltPassword(Password);
            }

            [Test]
            public void ThenTheHashedAndSaltedPasswordShouldBeDifferent() 
                => Assert.AreNotEqual(_hashedAndSaltedPassword, Password);
        }
    }
}
