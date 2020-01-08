using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.RequestValidatorTests
{
    public class ValidateAcceptRequestTests
    {
        public class GivenANonExistentRequestAndAlreadyFriends : RequestValidatorTestBase
        {
            private Request _request;

            private IList<Error> _result;

            private IList<Error> _expectedErrors;

            [OneTimeSetUp]
            public async Task WhenValidateAcceptRequestGetsCalled()
            {
                Setup();

                _expectedErrors = new List<Error>
                {
                    new Error("invalid_request", "There is no request to accept."),
                    new Error("invalid_request", "You are already connected.")
                };

                _request = new Fixture().Build<Request>().With(request => request.RequestType, RequestType.Friend).Create();

                RequestsRepository.Setup(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()))
                    .ReturnsAsync((Request) null);
                RelationshipsService.Setup(service => service.AreAlreadyFriends(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ReturnsAsync(true);

                _result = await RequestValidator.ValidateAcceptRequest(_request);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedLength()
                => Assert.That(_result.Count, Is.EqualTo(2));

            [Test]
            public void ThenErrorsShouldBeAsExpected()
                => _result.Should().BeEquivalentTo(_expectedErrors);

            [Test]
            public void ThenRequestsRepositoryGetRequestByUserIdsAndTypeShouldHaveBeenCalled()
                => RequestsRepository.Verify(repository => repository.GetRequestByUserIdsAndType(It.IsAny<Request>()), Times.Once);

            [Test]
            public void ThenRelationshipsServiceAreAlreadyFriendsShouldHaveBeenCalled()
                => RelationshipsService.Verify(service => service.AreAlreadyFriends(_request.SenderId, _request.ReceiverId), Times.Once);
        }
    }
}
