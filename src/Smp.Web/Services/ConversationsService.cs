using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IConversationsService
    {
        Task<IList<Conversation>> GetConversations(Guid userId);
    }

    public class ConversationsService : IConversationsService
    {
        private readonly IConversationsRepository _conversationsRepository;

        public ConversationsService(IConversationsRepository conversationsRepository)
        {
            _conversationsRepository = conversationsRepository;
        }

        public async Task<IList<Conversation>> GetConversations(Guid userId)
        {
            var conversationParticipants = await _conversationsRepository.GetConversationParticipantsByUserId(userId);

            return await _conversationsRepository.GetConversationsByIds(conversationParticipants.Select(ptcp => ptcp.ConversationId).ToList());
        }
    }
}