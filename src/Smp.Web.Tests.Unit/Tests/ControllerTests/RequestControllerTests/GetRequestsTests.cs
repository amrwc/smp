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

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestControllerTests
{
    [TestFixture]
    public class GetRequestsTests
    {
        [TestFixture]
        public class GivenAUserWithRequests : RequestControllerTestBase
        {
            private Guid _userId = Guid.NewGuid();
            private IList<Request> _expectedRequests;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRequestsGetCalled()
            {
                Setup();

                var fixture = new Fixture();
                _expectedRequests = fixture.CreateMany<Request>().ToList();

                RequestRepository.Setup(repo => repo.GetRequestsBySenderId(It.IsAny<Guid>())).ReturnsAsync(_expectedRequests);

                _result = await RequestController.GetRequests(_userId);
            }

            [Test]
            public void ThenRequestRepositoryGetRequestsBySenderIdShouldHaveBeenCalled()
                => RequestRepository.Verify(repo => repo.GetRequestsBySenderId(_userId), Times.Once);

            [Test]
            public void ThenOkResultShouldHaveBeenRetuned()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeListOfRequests()
                => Assert.IsInstanceOf<List<Request>>(((OkObjectResult)_result).Value);

            [Test]
            public void ThenRequestsShouldBeAsExpected()
                => ActualResults.Should().BeEquivalentTo(_expectedRequests);

            private IList<Request> ActualResults
                => (List<Request>)(((OkObjectResult)_result).Value);
        }
    }
}
