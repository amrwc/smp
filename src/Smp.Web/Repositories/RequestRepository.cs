using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IRequestRepository
    {
        Task CreateRequest(Request newRequest);
        Task DeleteRequest(Guid userOneId, Guid userTwoId,  RequestType requestType);
        Task<IList<Request>> GetRequestsByUserIds(Guid userOneId, Guid userTwoId);
        Task<IList<Request>> GetRequestsBySenderId(Guid SenderId);
        Task<Request> GetRequestByUserIdsAndType(Request request);
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
                new {newRequest.SenderId, newRequest.ReceiverId, newRequest.SentDate, newRequest.RequestType});
        }

        public async Task DeleteRequest(Guid userOneId, Guid userTwoId, RequestType requestType)
        {
            await _dbConnection.ExecuteAsync(
@"DELETE FROM [dbo].[Requests]
WHERE ([SenderId] = @UserOneId AND [ReceiverId] = @UserTwoId AND [RequestTypeId] = @RequestTypeId)
OR ([SenderId] = @UserTwoId AND [ReceiverId] = @UserOneId AND [RequestTypeId] = @RequestTypeId)",
                new {UserOneId = userOneId, UserTwoId = userTwoId, RequestTypeId = requestType});
        }

        public async Task<IList<Request>> GetRequestsByUserIds(Guid userOneId, Guid userTwoId)
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.Request>(
@"SELECT [SenderId], [ReceiverId] FROM [dbo].Requests
WHERE ([SenderId] = @UserOneId AND [ReceiverId] = @UserTwoId)
OR ([SenderId] = @UserTwoId AND [ReceiverId] = @UserOneId)",
                new {UserOneId = userOneId, UserTwoId = userTwoId}))
                    .Select(req => (Request) req).ToList();
        }

        public async Task<IList<Request>> GetRequestsBySenderId(Guid SenderId)
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.Request>(
                "SELECT [SenderId], [ReceiverId], [SentDate], [RequestTypeId] FROM [dbo].Requests WHERE [SenderId] = @SenderId",
                new {SenderId = SenderId})).Select(req => (Request) req).ToList();
        }

        public async Task<Request> GetRequestByUserIdsAndType(Request request)
        {
            return (Request) await _dbConnection.QueryFirstAsync<Models.DTOs.Request>(
@"SELECT TOP 1 [SenderId], [ReceiverId], [RequestTypeId] FROM [dbo].Requests
WHERE ([SenderId] = @SenderId AND [ReceiverId] = @ReceiverId AND [RequestTypeId] = @RequestTypeId)
OR ([SenderId] = @ReceiverId AND [ReceiverId] = @SenderId AND [RequestTypeId] = @RequestTypeId)",
                new {SenderId = request.SenderId, ReceiverId = request.ReceiverId, RequestTypeId = (short) request.RequestType});
        }
    }
}