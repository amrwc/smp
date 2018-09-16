using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;
using Smp.Web.Models.Results;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AuthControllerTests
{
    [TestFixture]
    public class SignInTests
    {
        [TestFixture]
        public class GivenValidCredentials : AuthControllerTestBase
        {
            private SignInRequest _signInRequest;
            private User _user;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenSignInGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _signInRequest = fixture.Create<SignInRequest>();
                _user = fixture.Create<User>();

                AuthService.Setup(service => service.VerifyUser(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new VerifyUserResult(true, _user)));

                _result = await AuthController.SignIn(_signInRequest);
            }

            [Test]
            public void ThenResultShouldBeOkObjectResult()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenAuthServiceVerifyUserShouldHaveBeenCalled()
                => AuthService.Verify(service => service.VerifyUser(_signInRequest.Email, _signInRequest.Password), Times.Once);
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
