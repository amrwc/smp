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
    public interface IMessagesRepository
    {
        Task CreateMessage(Message message);
        Task<IList<Message>> GetMessagesByConversationId(Guid conversationId, int count, int page, bool ascending);
    }

    [ExcludeFromCodeCoverage]
    public class MessagesRepository : IMessagesRepository
    {
        private readonly IDbConnection _dbConnection;

        public MessagesRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreateMessage(Message message)
        {
            await _dbConnection.ExecuteAsync("INSERT INTO [dbo].[Messages] ([SenderId], [Content], [CreatedAt], [ConversationId]) VALUES (@SenderId, @Content, @CreatedAt, @ConversationId)",
                new { SenderId = message.SenderId, Content = message.Content, CreatedAt = message.CreatedAt, ConversationId = message.ConversationId });
        }

        public async Task<IList<Message>> GetMessagesByConversationId(Guid conversationId, int count, int page, bool ascending)
        {
            var messages = await _dbConnection.QueryAsync<Models.DTOs.Message>(
                $@"SELECT * FROM [Messages] WHERE [ConversationId] = @ConversationId
                ORDER BY [Id] {(ascending ? "ASC" : "DESC")} OFFSET (@Skip) ROWS FETCH NEXT (@Count) ROWS ONLY",
                new { ConversationId = conversationId, Skip = count * page, Count = count });

            return messages.Select(msg => (Message)msg).ToList();
        }
    }
}
