using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IConversationsRepository
    {
        Task<IList<Conversation>> GetConversationsByIds(IEnumerable<Guid> conversationIds);
        Task<IList<ConversationParticipant>> GetConversationParticipantsByUserId(Guid userId);
    }

    [ExcludeFromCodeCoverage]
    public class ConversationsRepository : IConversationsRepository
    {
        private readonly IDbConnection _dbConnection;

        public ConversationsRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task<IList<Conversation>> GetConversationsByIds(IEnumerable<Guid> conversationIds)
        {
            var conversations = await _dbConnection.QueryAsync<Models.DTOs.Conversation>("SELECT * FROM [Conversations] WHERE [Id] IN @Ids",
                new { Ids = conversationIds });

            return conversations.Select(cnv => (Conversation)cnv).ToList();
        }

        public async Task<IList<ConversationParticipant>> GetConversationParticipantsByUserId(Guid userId)
        {
            var conversationParticipants = await _dbConnection.QueryAsync<Models.DTOs.ConversationParticipant>(
                "SELECT * FROM [ConversationParticipants] WHERE [UserId] = @UserId",
                new { UserId = userId }
            );

            return conversationParticipants.Select(ptcp => (ConversationParticipant)ptcp).ToList();
        }
    }
}