using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    [TestFixture]
    public class AcceptRequestTests
    {
        [TestFixture]
        public class GivenAValidRequest : RequestsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();
            private readonly Guid _senderId = Guid.NewGuid();
            private const byte RequestTypeId = 1;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenAcceptRequestsGetCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestValidator.Setup(validator => validator.ValidateAcceptRequest(It.IsAny<Request>())).ReturnsAsync(new List<Error>());

                _result = await RequestsController.AcceptRequest(_userId, _senderId, RequestTypeId);
            }

            [Test]
            public void ThenRequestServiceAcceptRequestShouldHaveBeenCalled()
                => RequestsService.Verify(service => service.AcceptRequest(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenResultShouldBeOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }
    }
}
