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

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.MessagesControllerTests
{
    [TestFixture]
    public class GetMessagesFromConversationTests
    {
        [TestFixture]
        public class GivenAnInvalidUserIdInToken : MessagesControllerTestBase
        {
            private readonly Guid _conversationId = Guid.NewGuid();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetConversationParticipantsGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.GetUserIdFromToken(It.IsAny<string>())).Returns("not a guid");

                _result = await MessagesController.GetMessagesFromConversation(_conversationId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);
        }

        [TestFixture]
        public class GivenAUserThatIsNotInTheConversation : MessagesControllerTestBase
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

                _result = await MessagesController.GetMessagesFromConversation(_conversationId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenConversationsServiceGetConversationParticipantsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.GetConversationParticipants(_conversationId), Times.Once);
        }

        [TestFixture]
        public class GivenAValidRequest : MessagesControllerTestBase
        {
            private readonly Guid _conversationId = Guid.NewGuid();
            private Guid _userId;

            private IActionResult _result;

            private IList<Guid> _conversationParticipants;
            private IList<Message> _messages;

            [OneTimeSetUp]
            public async Task WhenGetConversationParticipantsGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _conversationParticipants = fixture.CreateMany<Guid>().ToList();
                _messages = fixture.CreateMany<Message>().ToList();
                _userId = _conversationParticipants.Last();

                AuthService.Setup(service => service.GetUserIdFromToken(It.IsAny<string>())).Returns(_userId.ToString());
                ConversationsService.Setup(service => service.GetConversationParticipants(It.IsAny<Guid>()))
                    .ReturnsAsync(_conversationParticipants);
                MessagesService.Setup(service => service.GetMessagesFromConversation(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(_messages);

                _result = await MessagesController.GetMessagesFromConversation(_conversationId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => (_result as OkObjectResult)!.Value.Should().BeEquivalentTo(_messages);

            [Test]
            public void ThenConversationsServiceGetConversationParticipantsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.GetConversationParticipants(_conversationId), Times.Once);
        }
    }
}
