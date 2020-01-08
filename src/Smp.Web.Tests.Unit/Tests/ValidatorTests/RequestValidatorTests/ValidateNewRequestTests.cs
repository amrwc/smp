using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.RequestValidatorTests
{
    public class ValidateNewRequestTests
    {
        public class GivenAnInvalidRequest : RequestValidatorTestBase
        {
            private Request _request;

            private IList<Error> _result;

            private IList<Error> _expectedErrors;

            [OneTimeSetUp]
            public async Task WhenValidateNewRequestsGetsCalled()
            {
                Setup();

                _expectedErrors = new List<Error>
                {
                    new Error("invalid_request", "A user cannot send themselves a request."),
                    new Error("invalid_request", "You are already connected."),
                    new Error("invalid_request", "The request has already been sent.")
                };

                _request = new Fixture().Build<Request>().With(request => request.RequestType, RequestType.Friend)
                    .Create();
                _request.ReceiverId = _request.SenderId;

                RelationshipsService.Setup(service => service.AreAlreadyFriends(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ReturnsAsync(true);
                RequestsService.Setup(service => service.IsRequestAlreadySent(It.IsAny<Request>())).ReturnsAsync(true);

                _result = await RequestValidator.ValidateNewRequest(_request);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedLength()
                => Assert.That(_result.Count, Is.EqualTo(3));

            [Test]
            public void ThenErrorsShouldBeAsExpected()
                => _result.Should().BeEquivalentTo(_expectedErrors);

            [Test]
            public void ThenRelationshipsServiceAreAlreadyFriendsShouldHaveBeenCalled()
                => RelationshipsService.Verify(service => service.AreAlreadyFriends(_request.SenderId, _request.ReceiverId), Times.Once);

            [Test]
            public void ThenRequestServiceIsRequestAlreadySentShouldHaveBeenCalled()
                => RequestsService.Verify(service => service.IsRequestAlreadySent(It.IsAny<Request>()), Times.Once);
        }
    }
}
