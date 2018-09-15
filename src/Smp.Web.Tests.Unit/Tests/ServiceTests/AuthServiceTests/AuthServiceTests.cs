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
        public class GivenAnEmailAndPassword
        {
            protected IAuthService AuthService = new AuthService();
            
            [OneTimeSetUp]
            public void WhenSignInGetsCalled()
            {
                
            }
        }
    }
}