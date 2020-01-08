using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.RequestsServiceTests
{
    public class IsRequestAlreadySentTests
    {
        public class GivenAnAlreadySentRequest : RequestsServiceTestBase
        {
            private Request _request;
            private Request _usedRequest;

            private bool _result;

            [OneTimeSetUp]
            public async Task WhenIsRequestAlreadySentGetsCalled()
            {
                Setup();

                _request = new Fixture().Create<Request>();

                RequestsRepository.Setup(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()))
                    .ReturnsAsync(_request).Callback<Request>(req => _usedRequest = req);

                _result = await RequestsService.IsRequestAlreadySent(_request);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => Assert.IsTrue(_result);

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldHaveBeenCalled()
                => RequestsRepository.Verify(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenTheUsedRequestShouldBeAsExpected()
                => _usedRequest.Should().BeEquivalentTo(_request);
        }
    }
}
