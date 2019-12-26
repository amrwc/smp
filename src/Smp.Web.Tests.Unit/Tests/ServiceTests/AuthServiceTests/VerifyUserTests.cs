using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Results;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AuthServiceTests
{
    [TestFixture]
    public class VerifyUserTests
    {
        [TestFixture]
        public class GivenACorrectEmailAndPassword : AuthServiceTestBase
        {
            private const string _email = "hey";
            private const string _password = "hey";
            private VerifyUserResult _verifyUserResult;

            [OneTimeSetUp]
            public async Task WhenVerifyUserGetsCalled()
            {
                Setup();

                UserRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(new User {Password = "XD" }));

                CryptographyService.Setup(service => service.CheckPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
                _verifyUserResult = await AuthService.VerifyUser(_email, _password);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => Assert.IsTrue(_verifyUserResult.Success);

            [Test]
            public void ThenUserRepositoryGetUserShouldHaveBeenCalled()
                => UserRepository.Verify(repository => repository.GetUserByEmail(_email), Times.Once);

            [Test]
            public void ThenCryptographyServiceCheckPasswordShouldHaveBeenCalled()
                => CryptographyService.Verify(service => service.CheckPassword(_password, "XD"), Times.Once);
        }

        [TestFixture]
        public class GivenAnIncorrectEmailAndPassword : AuthServiceTestBase
        {
            private const string _email = "hey";
            private const string _password = "hey";
            private VerifyUserResult _verifyUserResult;

            [OneTimeSetUp]
            public async Task WhenVerifyUserGetsCalled()
            {
                Setup();

                UserRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(new User {Password = "XD" }));

                CryptographyService.Setup(service => service.CheckPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
                _verifyUserResult = await AuthService.VerifyUser(_email, _password);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => Assert.IsFalse(_verifyUserResult.Success);

            [Test]
            public void ThenUserRepositoryGetUserShouldHaveBeenCalled()
                => UserRepository.Verify(repository => repository.GetUserByEmail(_email), Times.Once);

            [Test]
            public void ThenCryptographyServiceCheckPasswordShouldHaveBeenCalled()
                => CryptographyService.Verify(service => service.CheckPassword(_password, "XD"), Times.Once);
        }
    }
}