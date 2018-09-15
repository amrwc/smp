using System.Collections.Generic;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.CryptographyServiceTests
{
    [TestFixture]
    public class CheckPasswordTests
    {
        [TestFixture]
        public class GivenAPasswordAndItsHashedVersion : CryptographyServiceTestBase
        {
            private const string Password = "hello";
            private string _hashedAndSaltedPassword;

            private bool _isPasswordCorrect;

            [OneTimeSetUp]
            public void WhenCheckPasswordGetsCalled()
            {
                Setup();

                _hashedAndSaltedPassword = BCrypt.Net.BCrypt.HashPassword(Password, BCrypt.Net.BCrypt.GenerateSalt());

                _isPasswordCorrect = CryptographyService.CheckPassword(Password, _hashedAndSaltedPassword);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => Assert.IsTrue(_isPasswordCorrect);
        }

        [TestFixture]
        public class GivenAPasswordAndADifferentHashedPassword : CryptographyServiceTestBase
        {
            private const string Password = "hello";
            private string _hashedAndSaltedPassword;

            private bool _isPasswordCorrect;

            [OneTimeSetUp]
            public void WhenCheckPasswordGetsCalled()
            {
                Setup();

                _hashedAndSaltedPassword = BCrypt.Net.BCrypt.HashPassword("somethingelse", BCrypt.Net.BCrypt.GenerateSalt());

                _isPasswordCorrect = CryptographyService.CheckPassword(Password, _hashedAndSaltedPassword);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => Assert.IsFalse(_isPasswordCorrect);
        }
    }
}
