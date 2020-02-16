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
    public class GetIncomingRequestsTests
    {
        [TestFixture]
        public class GivenAUserWithRequests : RequestsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();
            private IList<Request> _expectedRequests;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetIncomingRequestsGetCalled()
            {
                Setup();

                var fixture = new Fixture();
                _expectedRequests = fixture.CreateMany<Request>().ToList();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RequestsRepository.Setup(repo => repo.GetRequestsByReceiverId(It.IsAny<Guid>())).ReturnsAsync(_expectedRequests);

                _result = await RequestsController.GetIncomingRequests(_userId);
            }

            [Test]
            public void ThenRequestRepositoryGetRequestsBySenderIdShouldHaveBeenCalled()
                => RequestsRepository.Verify(repo => repo.GetRequestsByReceiverId(_userId), Times.Once);

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeListOfRequests()
                => Assert.IsInstanceOf<List<Request>>(((OkObjectResult)_result).Value);

            [Test]
            public void ThenRequestsShouldBeAsExpected()
                => ActualResults.Should().BeEquivalentTo(_expectedRequests);

            private IList<Request> ActualResults
                => (List<Request>)((OkObjectResult)_result).Value;
        }
    }
}
