using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    [TestFixture]
    public class DeclineRequestTests
    {
        [TestFixture]
        public class GivenAValidRequest : RequestsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();
            private readonly Guid _senderId = Guid.NewGuid();
            private const byte RequestTypeId = 1;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenDeclineRequestsGetCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestsRepository
                    .Setup(repository => repository
                        .GetRequestByUserIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RequestType>()))
                    .ReturnsAsync(new Request());

                _result = await RequestsController.DeclineRequest(_userId, _senderId, RequestTypeId);
            }

            [Test]
            public void ThenRequestServiceDeclineRequestShouldHaveBeenCalled()
                => RequestsService.Verify(service => service.DeclineRequest(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldHaveBeenCalled()
                => RequestsRepository.Verify(
                    repository =>
                        repository.GetRequestByUserIdsAndType(_userId, _senderId, (RequestType)RequestTypeId), Times.Once);

            [Test]
            public void ThenResultShouldBeOkResult()
                => Assert.IsInstanceOf<OkResult>(_result);
        }

        [TestFixture]
        public class GivenAnUnauthorizedRequest : RequestsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();
            private readonly Guid _senderId = Guid.NewGuid();
            private const byte RequestTypeId = 1;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenDeclineRequestsGetCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(false);

                _result = await RequestsController.DeclineRequest(_userId, _senderId, RequestTypeId);
            }

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldNotHaveBeenCalled()
                => RequestsRepository.Verify(
                    repository =>
                        repository.GetRequestByUserIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(),
                            It.IsAny<RequestType>()), Times.Never);

            [Test]
            public void ThenResultShouldBeUnauthorizedResult()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);
        }

        [TestFixture]
        public class GivenANonExistentRequest : RequestsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();
            private readonly Guid _senderId = Guid.NewGuid();
            private const byte RequestTypeId = 1;
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenDeclineRequestsGetCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestsRepository
                    .Setup(repository => repository
                        .GetRequestByUserIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RequestType>()))
                    .ReturnsAsync((Request)null);

                _result = await RequestsController.DeclineRequest(_userId, _senderId, RequestTypeId);
            }

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldHaveBeenCalled()
                => RequestsRepository.Verify(
                    repository =>
                        repository.GetRequestByUserIdsAndType(_userId, _senderId, (RequestType) RequestTypeId),
                    Times.Once);

            [Test]
            public void ThenRequestServiceDeclineRequestShouldNotHaveBeenCalled()
                => RequestsService.Verify(service => service.DeclineRequest(It.IsAny<Request>()), Times.Never);

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<BadRequestObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => (_result as BadRequestObjectResult)!.Value.Should()
                    .BeEquivalentTo(new Error("invalid_request", "There is no pending request."));
        }
    }
}
