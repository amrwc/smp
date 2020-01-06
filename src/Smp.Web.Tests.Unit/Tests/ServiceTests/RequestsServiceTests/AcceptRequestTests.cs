using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.RequestsServiceTests
{
    public class AcceptRequestTests
    {
        public class GivenAFriendRequestToAccept : RequestsServiceTestBase
        {
            private Request _request;

            [OneTimeSetUp]
            public async Task WhenAcceptRequestGetsCalled()
            {
                Setup();

                _request = new Fixture().Build<Request>().With(request => request.RequestType, RequestType.Friend).Create();

                await RequestsService.AcceptRequest(_request);
            }

            [Test]
            public void ThenRelationshipsServiceAddFriendShouldHaveBeenCalled()
                => RelationshipsService.Verify(service => service.AddFriend(_request.SenderId, _request.ReceiverId), Times.Once);

            [Test]
            public void ThenRequestsRepositoryDeleteRequestShouldHaveBeenCalled()
                => RequestsRepository.Verify(
                    repository => repository.DeleteRequest(_request.SenderId, _request.ReceiverId, RequestType.Friend), Times.Once);
        }
    }
}
