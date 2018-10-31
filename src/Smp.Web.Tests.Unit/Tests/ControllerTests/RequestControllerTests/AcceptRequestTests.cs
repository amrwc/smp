using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestControllerTests
{
    [TestFixture]
    public class AcceptRequestTests
    {
        [TestFixture]
        public class GivenAValidRequest : RequestControllerTestBase
        {
            private Guid _userId = Guid.NewGuid();
            private Guid _senderId = Guid.NewGuid();
            private RequestType _requestType = RequestType.Friend;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenAcceptRequestsGetCalled()
            {
                Setup();

                RequestService.Setup(service => service.ValidateAcceptRequest(It.IsAny<Request>())).Returns(Task.FromResult(new List<Error>()));
                _result = await RequestController.AcceptRequest(_userId, _senderId, (int) _requestType);
            }

            [Test]
            public void ThenRequestServiceAcceptRequestShouldHaveBeenCalled()
                => RequestService.Verify(service => service.AcceptRequest(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenResultShouldBeOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }
    }
}
