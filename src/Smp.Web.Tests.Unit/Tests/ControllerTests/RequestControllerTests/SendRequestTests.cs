using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestControllerTests
{
    [TestFixture]
    public class SendRequestTests
    {
        [TestFixture]
        public class GivenAValidFriendRequest : RequestControllerTestBase
        {
            private RequestRequest _requestRequest;

            private Request _request;

            private IActionResult _result;

            [OneTimeSetUp]
            public void WhenSendFriendRequestGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _requestRequest = fixture.Create<RequestRequest>();

                _result = RequestController.SendRequest(_requestRequest);
            }

            [Test]
            public void ThenRequestServiceValidateRequestShouldHaveBeenCalled() 
                => RequestService.Verify(service => service.ValidateRequest(_request), Times.Once);
        }
    }
}
