using System.Collections.Generic;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AuthServiceTests
{
    [TestFixture]
    public class VerifyUserTests
    {
        [TestFixture]
        public class GivenAnEmailAndPassword : AuthServiceTestBase
        {
            private string _email;
            private string _password;

            private bool _isMatch;

            [OneTimeSetUp]
            public void WhenVerifyUserGetsCalled()
            {
                Setup();

                UserRepository.Setup(repo => repo.GetUser(It.IsAny<string>()).Returns(new User { Email = _email, Password = _password }));

                _isMatch = AuthService.VerifyUser(_email, _password);
            }
        }
    }
}