using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.ConversationsServiceTests
{
    [TestFixture]
    public class GetConversationParticipantsTests
    {
        [TestFixture]
        public class GivenAValidCall : ConversationsServiceTestBase
        {
            private readonly Guid _conversationId = Guid.NewGuid();

            private IList<ConversationParticipant> _conversationParticipants;

            private IList<Guid> _participantIds;

            [OneTimeSetUp]
            public async Task WhenGetConversationParticipantsGetsCalled()
            {
                Setup();

                _conversationParticipants = new Fixture().CreateMany<ConversationParticipant>().ToList();

                ConversationsRepository.Setup(repository =>
                        repository.GetConversationParticipantsByConversationId(It.IsAny<Guid>()))
                    .ReturnsAsync(_conversationParticipants);

                _participantIds = await ConversationsService.GetConversationParticipants(_conversationId);
            }

            [Test]
            public void ThenConversationsRepositoryGetConversationParticipantsByConversationIdShouldHaveBeenCalled()
                => ConversationsRepository.Verify(repository => repository.GetConversationParticipantsByConversationId(_conversationId), Times.Once);

            [Test]
            public void ThenResultShouldBeAsExpected()
                => _participantIds.Should().BeEquivalentTo(_conversationParticipants.Select(participant => participant.UserId).ToList());
        }
    }
}
