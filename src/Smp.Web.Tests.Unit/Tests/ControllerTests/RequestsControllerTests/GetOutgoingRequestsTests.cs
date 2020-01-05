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
    public class GetOutgoingRequestsTests
    {
        [TestFixture]
        public class GivenAUserWithRequests : RequestsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();

            private IActionResult _result;

            private IList<Request> _requests;

            [OneTimeSetUp]
            public async Task WhenGetOutgoingRequestsGetCalled()
            {
                Setup();

                _requests = new Fixture().CreateMany<Request>().ToList();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestsRepository.Setup(repository => repository.GetRequestsBySenderId(_userId))
                    .ReturnsAsync(_requests);

                _result = await RequestsController.GetOutgoingRequests(_userId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => ((OkObjectResult) _result).Value.Should().BeEquivalentTo(_requests);

            [Test]
            public void ThenRequestsRepositoryGetRequestsBySenderIdShouldHaveBeenCalled()
                => RequestsRepository.Verify(repository => repository.GetRequestsBySenderId(_userId), Times.Once);
        }
    }
}
