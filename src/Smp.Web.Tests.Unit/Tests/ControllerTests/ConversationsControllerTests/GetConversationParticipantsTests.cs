using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.ConversationsControllerTests
{
    [TestFixture]
    public class GetConversationParticipantsTests
    {
        [TestFixture]
        public class GivenAnInvalidUserIdInToken : ConversationsControllerTestBase
        {
            private readonly Guid _conversationId = Guid.NewGuid();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetConversationParticipantsGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.GetUserIdFromToken(It.IsAny<string>())).Returns("not a guid");

                _result = await ConversationsController.GetConversationParticipants(_conversationId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);
        }

        [TestFixture]
        public class GivenAUserThatIsNotInTheConversation : ConversationsControllerTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();
            private readonly Guid _conversationId = Guid.NewGuid();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetConversationParticipantsGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.GetUserIdFromToken(It.IsAny<string>())).Returns(_userId.ToString());
                ConversationsService.Setup(service => service.GetConversationParticipants(It.IsAny<Guid>()))
                    .ReturnsAsync(new Fixture().CreateMany<Guid>().ToList());

                _result = await ConversationsController.GetConversationParticipants(_conversationId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenConversationsServiceGetConversationParticipantsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.GetConversationParticipants(_conversationId), Times.Once);
        }

        [TestFixture]
        public class GivenAValidRequest : ConversationsControllerTestBase
        {
            private readonly Guid _conversationId = Guid.NewGuid();
            private Guid _userId;

            private IActionResult _result;

            private IList<Guid> _conversationParticipants;

            [OneTimeSetUp]
            public async Task WhenGetConversationParticipantsGetsCalled()
            {
                Setup();

                _conversationParticipants = new Fixture().CreateMany<Guid>().ToList();
                _userId = _conversationParticipants.Last();

                AuthService.Setup(service => service.GetUserIdFromToken(It.IsAny<string>())).Returns(_userId.ToString());
                ConversationsService.Setup(service => service.GetConversationParticipants(It.IsAny<Guid>()))
                    .ReturnsAsync(_conversationParticipants);

                _result = await ConversationsController.GetConversationParticipants(_conversationId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => (_result as OkObjectResult)!.Value.Should().BeEquivalentTo(_conversationParticipants);

            [Test]
            public void ThenConversationsServiceGetConversationParticipantsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.GetConversationParticipants(_conversationId), Times.Once);
        }
    }
}
