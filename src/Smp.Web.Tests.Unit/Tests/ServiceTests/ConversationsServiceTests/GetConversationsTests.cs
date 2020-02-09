using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.ConversationsServiceTests
{
    [TestFixture]
    public class GetConversationsTests
    {
        [TestFixture]
        public class GivenAValidCall : ConversationsServiceTestBase
        {
            private readonly Guid _userId = Guid.NewGuid();

            private IList<ConversationParticipant> _conversationParticipants;
            private IList<Guid> _usedConversationIds;

            private IList<Conversation> _expectedConversations;

            private IList<Conversation> _conversations;

            [OneTimeSetUp]
            public async Task WhenGetConversationsGetsCalled()
            {
                Setup();

                var fixture = new Fixture();
                _conversationParticipants = fixture.CreateMany<ConversationParticipant>().ToList();
                _expectedConversations = fixture.CreateMany<Conversation>().ToList();

                ConversationsRepository.Setup(repository => repository.GetConversationParticipantsByUserId(It.IsAny<Guid>()))
                    .ReturnsAsync(_conversationParticipants);
                ConversationsRepository.Setup(repository => repository.GetConversationsByIds(It.IsAny<IList<Guid>>()))
                    .ReturnsAsync(_expectedConversations).Callback<IList<Guid>>(ids => _usedConversationIds = ids);

                _conversations = await ConversationsService.GetConversations(_userId);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => _conversations.Should().BeEquivalentTo(_expectedConversations);

            [Test]
            public void ThenConversationsRepositoryGetConversationParticipantsByUserId()
                => ConversationsRepository.Verify(repository => repository.GetConversationParticipantsByUserId(_userId),
                    Times.Once);

            [Test]
            public void ThenConversationsRepositoryGetConversationsByIdsShouldHaveBeenCalled()
                => ConversationsRepository.Verify(
                    repository =>
                        repository.GetConversationsByIds(It.IsAny<IList<Guid>>()), Times.Once);

            [Test]
            public void ThenUsedConversationIdsShouldBeAsExpected()
                => _usedConversationIds.Should()
                    .BeEquivalentTo(_conversationParticipants.Select(ptcp => ptcp.ConversationId).ToList());
        }
    }
}
