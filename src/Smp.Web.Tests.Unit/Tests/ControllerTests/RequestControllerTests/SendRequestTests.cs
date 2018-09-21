using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Repositories;

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
            public async Task WhenSendFriendRequestGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _request = fixture.Create<Request>();
                _requestRequest = fixture.Create<RequestRequest>();

                RequestService.Setup(service => service.ValidateRequest(It.IsAny<Request>())).Returns(new List<Error>());

                _result = await RequestController.SendRequest(_requestRequest);
            }

            [Test]
            public void ThenRequestServiceValidateRequestShouldHaveBeenCalled()
                => RequestService.Verify(service => service.ValidateRequest(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenRequestRepositoryCreateRequestShouldHaveBeenCalled()
                => RequestRepository.Verify(repo => repo.CreateRequest(It.Is<Request>(request =>
                    request.SenderId == _requestRequest.SenderId && request.ReceiverId == _requestRequest.ReceiverId &&
                    request.SentDate == _requestRequest.SentDate && request.RequestTypeId == _requestRequest.RequestTypeId)));

            [Test]
            public void ThenResultShouldBeAnOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }
    }
}
