using System;
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
    public class CreateMessageTests
    {
        public class GivenAnAuthorizedRequest : MessagesControllerTestBase
        {
            private CreateMessageRequest _createMessageRequest;

            private IActionResult _result;

            private Message _usedMessage;

            [OneTimeSetUp]
            public async Task WhenCreateMessageGetsCalled()
            {
                Setup();

                _createMessageRequest = new Fixture().Create<CreateMessageRequest>();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>()))
                    .Returns(true);
                AuthService.Setup(service => service.AuthorizeFriend(It.IsAny<string>(), It.IsAny<Guid>()))
                    .ReturnsAsync(true);
                MessagesRepository.Setup(repository => repository.CreateMessage(It.IsAny<Message>()))
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
                => MessagesRepository.Verify(repository => repository.CreateMessage(It.IsAny<Message>()), Times.Once);
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
                => MessagesRepository.Verify(repository => repository.CreateMessage(It.IsAny<Message>()), Times.Never);
        }
    }
}
