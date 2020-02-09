using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.ConversationsControllerTests
{
    [TestFixture]
    public class GetConversationsTests
    {
        [TestFixture]
        public class GivenAValidRequest : ConversationsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetConversationsGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                ConversationsService.Setup(service => service.GetConversations(It.IsAny<Guid>()))
                    .ReturnsAsync(new List<Conversation>());

                _result = await ConversationsController.GetConversations(_userId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => (_result as OkObjectResult)!.Value.Should().BeEquivalentTo(new List<Conversation>());

            [Test]
            public void ThenConversationsServiceGetConversationsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.GetConversations(_userId), Times.Once);
        }

        [TestFixture]
        public class GivenAnUnauthorizedRequest : ConversationsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetConversationsGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(false);

                _result = await ConversationsController.GetConversations(_userId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenConversationsServiceGetConversationsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.GetConversations(It.IsAny<Guid>()), Times.Never);
        }
    }
}
