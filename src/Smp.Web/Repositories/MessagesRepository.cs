using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IMessagesRepository
    {
        Task CreateMessage(Message message);
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
            await _dbConnection.ExecuteAsync("INSERT INTO [dbo].[Messages] ([SenderId], [ReceiverId], [Content], [CreatedAt]) VALUES (@SenderId, @ReceiverId, @Content, @CreatedAt)",
                new { SenderId = message.SenderId, ReceiverId = message.ReceiverId, Content = message.Content, CreatedAt = message.CreatedAt });
        }
    }
}