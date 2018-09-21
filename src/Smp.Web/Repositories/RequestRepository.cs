using System.Data;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IRequestRepository
    {
        Task CreateRequest(Request newRequest);
    }

    public class RequestRepository : IRequestRepository
    {
        private readonly IDbConnection _dbConnection;

        public RequestRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreateRequest(Request newRequest)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Requests] ([SenderId], [ReceiverId], [SentDate], [RequestTypeId]) VALUES (@SenderId, @ReceiverId, @SentDate, @RequestTypeId)",
                new {newRequest.SenderId, newRequest.ReceiverId, newRequest.SentDate, newRequest.RequestTypeId});
        }
    }
}