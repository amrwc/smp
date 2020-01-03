using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    [TestFixture]
    public class SendRequestTests
    {
        [TestFixture]
        public class GivenAValidFriendRequest : RequestsControllerTestBase
        {
            private RequestRequest _requestRequest;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenSendFriendRequestGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _requestRequest = fixture.Create<RequestRequest>();

                RequestsService.Setup(service => service.ValidateNewRequest(It.IsAny<Request>()))
                    .Returns(Task.FromResult(new List<Error>()));

                _result = await RequestsController.SendRequest(_requestRequest);
            }

            [Test]
            public void ThenRequestServiceValidateRequestShouldHaveBeenCalled()
                => RequestsService.Verify(service => service.ValidateNewRequest(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenRequestRepositoryCreateRequestShouldHaveBeenCalled()
                => RequestsRepository.Verify(repo => repo.CreateRequest(It.Is<Request>(request =>
                    request.SenderId == _requestRequest.SenderId && request.ReceiverId == _requestRequest.ReceiverId && request.RequestType == (RequestType)_requestRequest.RequestTypeId)));

            [Test]
            public void ThenResultShouldBeAnOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }

        [TestFixture]
        public class GivenARequestToFailValidation : RequestsControllerTestBase
        {
            private RequestRequest _requestRequest;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenSendFriendRequestGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _requestRequest = fixture.Create<RequestRequest>();

                RequestsService.Setup(service => service.ValidateNewRequest(It.IsAny<Request>()))
                    .Returns(Task.FromResult(new List<Error>{new Error(":^)", ":^)")}));

                _result = await RequestsController.SendRequest(_requestRequest);
            }

            [Test]
            public void ThenRequestServiceValidateRequestShouldHaveBeenCalled()
                => RequestsService.Verify(service => service.ValidateNewRequest(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenRequestRepositoryCreateRequestShouldNotHaveBeenCalled()
                => RequestsRepository.Verify(repo => repo.CreateRequest(It.IsAny<Request>()), Times.Never);

            [Test]
            public void ThenABadRequestShouldHaveBeenReturned()
                => Assert.IsInstanceOf<BadRequestObjectResult>(_result);
        }
    }
}
