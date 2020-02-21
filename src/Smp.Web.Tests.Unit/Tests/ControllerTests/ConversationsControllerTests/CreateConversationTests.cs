using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.ConversationsControllerTests
{
    [TestFixture]
    public class CreateConversationTest
    {
        [TestFixture("Hi Bob!", 1)]
        [TestFixture("", 0)]
        [TestFixture(null, 0)]
        public class GivenAValidRequest : ConversationsControllerTestBase
        {
            private readonly CreateConversationRequest _createConversationRequest = new CreateConversationRequest();
            private readonly int _timesCreateMessage;
            private readonly Guid _conversationId = Guid.NewGuid();
            private IActionResult _result;

            public GivenAValidRequest(string content, int timesCreateMessage)
            {
                _createConversationRequest.SenderId = Guid.NewGuid();
                _createConversationRequest.ReceiverId = Guid.NewGuid();
                _createConversationRequest.Content = content;
                _timesCreateMessage = timesCreateMessage;
            }

            [OneTimeSetUp]
            public async Task WhenCreateConversationGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(
                    It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                ConversationsService.Setup(service => service.CreateConversationWithParticipants(
                    _createConversationRequest.SenderId, _createConversationRequest.ReceiverId))
                    .ReturnsAsync(_conversationId);
                
                _result = await ConversationsController.CreateConversation(_createConversationRequest);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => Assert.AreEqual(_conversationId, (_result as OkObjectResult)!.Value);

            [Test]
            public void ThenConversationsServiceCreateConversationWithParticipantsShouldHaveBeenCalled()
                => ConversationsService.Verify(service => service.CreateConversationWithParticipants(
                    _createConversationRequest.SenderId, _createConversationRequest.ReceiverId), Times.Once);

            [Test]
            public void ThenMessagesServiceCreateMessageShouldHaveBeenCalled()
                => MessagesService.Verify(service => service.CreateMessage(It.IsAny<Message>()),
                    Times.Exactly(_timesCreateMessage));
        }

        [TestFixture]
        public class GivenAnUnauthorizedRequest : ConversationsControllerTestBase
        {
            private readonly CreateConversationRequest _createConversationRequest = new CreateConversationRequest();
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetConversationsGetsCalled()
            {
                Setup();
                _createConversationRequest.SenderId = Guid.NewGuid();
                _createConversationRequest.ReceiverId = Guid.NewGuid();
                _createConversationRequest.Content = "Hi Bob!";
                AuthService.Setup(service => service.AuthorizeSelf(
                    It.IsAny<string>(), It.IsAny<Guid>())).Returns(false);
                _result = await ConversationsController.CreateConversation(_createConversationRequest);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenConversationsServiceCreateConversationWithParticipantsShouldNotHaveBeenCalled()
                => ConversationsService.Verify(service => service.CreateConversationWithParticipants(
                    It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);

            [Test]
            public void ThenMessagesServiceCreateMessageShouldNotHaveBeenCalled()
                => MessagesService.Verify(service => service.CreateMessage(It.IsAny<Message>()), Times.Never);
        }
    }
}
