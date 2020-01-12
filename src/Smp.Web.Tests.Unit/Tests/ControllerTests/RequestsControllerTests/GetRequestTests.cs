using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    public class GetRequestTests
    {
        public class GivenAMatchingRequest : RequestsControllerTestBase
        {
            private const byte RequestTypeId = (byte) RequestType.Friend;
            private readonly Guid _receiverId = Guid.NewGuid();
            private readonly Guid _senderId = Guid.NewGuid();

            private IActionResult _result;

            private Request _request;

            private Request _expectedRequest;
            private Request _usedRequest;

            [OneTimeSetUp]
            public async Task WhenGetRequestGetsCalled()
            {
                Setup();

                _request = new Fixture().Create<Request>();
                _expectedRequest = new Request { ReceiverId = _receiverId, SenderId = _senderId, RequestType = (RequestType)RequestTypeId };

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestsRepository.Setup(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()))
                    .ReturnsAsync(_request).Callback<Request>(req => _usedRequest = req);

                _result = await RequestsController.GetRequest(_receiverId, _senderId, RequestTypeId);
            }

            [Test]
            public void ThenTheResultShouldBeOfTheExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenTheResultValueShouldBeAsExpected()
                => Assert.That(((OkObjectResult) _result).Value, Is.EqualTo(_request));

            [Test]
            public void ThenAuthServiceAuthorizeSelfShouldHaveBeenCalled()
                => AuthService.Verify(service => service.AuthorizeSelf(It.IsAny<string>(), _receiverId), Times.Once);

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldHaveBeenCalled()
                => RequestsRepository.Verify(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenUsedRequestShouldBeAsExpected()
            {
                _usedRequest.Should().BeEquivalentTo(_expectedRequest, options => options.Excluding(request => request.CreatedAt));
                _usedRequest.CreatedAt.Should().BeCloseTo(_expectedRequest.CreatedAt, 500);
            }
        }

        public class GivenNoMatchingRequest : RequestsControllerTestBase
        {
            private const byte RequestTypeId = (byte) RequestType.Friend;
            private readonly Guid _receiverId = Guid.NewGuid();
            private readonly Guid _senderId = Guid.NewGuid();

            private IActionResult _result;

            private Request _expectedRequest;
            private Request _usedRequest;

            [OneTimeSetUp]
            public async Task WhenGetRequestGetsCalled()
            {
                Setup();

                _expectedRequest = new Request
                    {ReceiverId = _receiverId, SenderId = _senderId, RequestType = (RequestType) RequestTypeId};

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestsRepository.Setup(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()))
                    .ReturnsAsync((Request) null).Callback<Request>(req => _usedRequest = req);

                _result = await RequestsController.GetRequest(_receiverId, _senderId, RequestTypeId);
            }

            [Test]
            public void ThenTheResultShouldBeOfTheExpectedType()
                => Assert.IsInstanceOf<NotFoundResult>(_result);

            [Test]
            public void ThenAuthServiceAuthorizeSelfShouldHaveBeenCalled()
                => AuthService.Verify(service => service.AuthorizeSelf(It.IsAny<string>(), _receiverId), Times.Once);

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldHaveBeenCalled()
                => RequestsRepository.Verify(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()),
                    Times.Once);

            [Test]
            public void ThenUsedRequestShouldBeAsExpected()
            {
                _usedRequest.Should().BeEquivalentTo(_expectedRequest,
                    options => options.Excluding(request => request.CreatedAt));
                _usedRequest.CreatedAt.Should().BeCloseTo(_expectedRequest.CreatedAt, 500);
            }
        }
    }
}
