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

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    [TestFixture]
    public class AcceptRequestTests
    {
        [TestFixture]
        public class GivenAValidRequest : RequestsControllerTestBase
        {
            private Guid _userId = Guid.NewGuid();
            private Guid _senderId = Guid.NewGuid();
            private byte _requestTypeId = 1;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenAcceptRequestsGetCalled()
            {
                Setup();

                RequestsService.Setup(service => service.ValidateAcceptRequest(It.IsAny<Request>())).Returns(Task.FromResult(new List<Error>()));
                _result = await RequestsController.AcceptRequest(_userId, _senderId, _requestTypeId);
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
