using System;
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
        Task<Friend> GetFriendByUserIds(Guid userOneId, Guid userTwoId);
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

        public async Task<Friend> GetFriendByUserIds(Guid userOneId, Guid userTwoId)
        {
            return (Friend) await _dbConnection.QueryFirstAsync<Models.DTOs.Friend>(
@"SELECT TOP 1 [UserOneId], [UserTwoId] FROM [dbo].[Friends]
WHERE ([UserOneId] = @UserOneId AND [UserTwoId] = @UserTwoId)
OR ([UserOneId] = @UserTwoId AND [UserTwoId] = @UserOneId)",
                new {UserOneId = userOneId, UserTwoId = userTwoId});
        }
    }
}