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
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.MessagesControllerTests
{
    [TestFixture]
    public class CreateMessageTests
    {
        public class GivenAnAuthorizedRequest : MessagesControllerTestBase
        {
            private CreateMessageRequest _createMessageRequest;
            private IList<Guid> _conversationParticipants;

            private IActionResult _result;

            private Message _usedMessage;

            [OneTimeSetUp]
            public async Task WhenCreateMessageGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _conversationParticipants = fixture.CreateMany<Guid>().ToList();
                _createMessageRequest = fixture.Build<CreateMessageRequest>()
                    .With(request => request.SenderId, _conversationParticipants.Last())
                    .Create();

                AuthService.Setup(service => service.GetUserIdFromToken(It.IsAny<string>())).Returns(_createMessageRequest.SenderId.ToString());
                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>()))
                    .Returns(true);
                ConversationsService.Setup(service => service.GetConversationParticipants(It.IsAny<Guid>()))
                    .ReturnsAsync(_conversationParticipants);
                MessagesService.Setup(repository => repository.CreateMessage(It.IsAny<Message>()))
                    .Callback<Message>(msg => _usedMessage = msg);

                _result = await MessagesController.CreateMessage(_createMessageRequest);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkResult>(_result);

            [Test]
            public void ThenUsedMessageShouldBeAsExpected()
                => _usedMessage.Should().BeEquivalentTo(new Message(_createMessageRequest), options => options.Excluding(msg => msg.CreatedAt));

            [Test]
            public void ThenMessagesRepositoryCreatePostShouldHaveBeenCalled()
                => MessagesService.Verify(repository => repository.CreateMessage(It.IsAny<Message>()), Times.Once);
        }

        public class GivenAnUnauthorizedRequest : MessagesControllerTestBase
        {
            private CreateMessageRequest _createMessageRequest;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenCreateMessageGetsCalled()
            {
                Setup();

                _createMessageRequest = new Fixture().Create<CreateMessageRequest>();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>()))
                    .Returns(false);

                _result = await MessagesController.CreateMessage(_createMessageRequest);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenMessagesRepositoryCreatePostShouldHaveBeenCalled()
                => MessagesService.Verify(repository => repository.CreateMessage(It.IsAny<Message>()), Times.Never);
        }
    }
}
