using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AuthControllerTests
{
    [TestFixture]
    public class SignInTests
    {
        [TestFixture]
        public class GivenValidCredentials : AuthControllerTestBase
        {
            private LoginRequest _loginRequest;

            private IActionResult _result;

            [OneTimeSetUp]
            public void WhenSignInGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _loginRequest = fixture.Create<LoginRequest>();

                _result = AuthController.SignIn(_loginRequest);
            }

            [Test]
            public void ThenResultShouldBeOkObjectResult() 
                => Assert.IsInstanceOf<OkObjectResult>(_result);
        }

//        [TestFixture]
//        public class GivenInvalidLoginCredentials : AuthControllerTestBase
//        {
//            private const string Email = "InvalidEmail";
//            private const string Password = "InvPass";
//
//            private IActionResult _result;
//
//            [OneTimeSetUp]
//            public void WhenLoginGetsCalled()
//            {
//                _result = AuthController.Login(Email, Password);
//            }
//
//            [Test]
//            public void ThenResultShouldBeOkObjectResult() 
//                => Assert.IsInstanceOf<OkObjectResult>(_result);
//        }
    }
}
