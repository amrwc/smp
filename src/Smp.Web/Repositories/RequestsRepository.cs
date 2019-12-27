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
    public interface IRequestsRepository
    {
        Task CreateRequest(Request newRequest);
        Task DeleteRequest(Guid userOneId, Guid userTwoId, byte requestTypeId);
        Task<IList<Request>> GetRequestsByUserIds(Guid userOneId, Guid userTwoId);
        Task<IList<Request>> GetRequestsBySenderId(Guid SenderId);
        Task<Request> GetRequestByUserIdsAndType(Request request);
        Task<IList<RequestType>> GetRequestTypes();
        Task<RequestType> GetRequestTypeById(byte id);
        Task<RequestType> GetRequestTypeByName(string name);
    }

    public class RequestsRepository : IRequestsRepository
    {
        private readonly IDbConnection _dbConnection;

        public RequestsRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task<IList<RequestType>> GetRequestTypes()
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.RequestType>(@"SELECT * FROM [dbo].[RequestTypes] SORT BY [Id] ASC"))
                .Select(reqType => (RequestType)reqType).ToList();
        }

        public async Task<RequestType> GetRequestTypeById(byte id)
        {
            return (RequestType)await _dbConnection.QueryFirstAsync<Models.DTOs.RequestType>(
                @"SELECT TOP 1 * FROM [dbo].[RequestTypes] WHERE [Id] = @Id",
                new { Id = id });
        }

        public async Task<RequestType> GetRequestTypeByName(string name)
        {
            return (RequestType)await _dbConnection.QueryFirstAsync<Models.DTOs.RequestType>(
                @"SELECT TOP 1 * FROM [dbo].[RequestTypes] WHERE [Type] = @RequestType",
                new { RequestType = name });
        }

        public async Task CreateRequest(Request newRequest)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Requests] ([SenderId], [ReceiverId], [CreatedAt], [RequestTypeId]) VALUES (@SenderId, @ReceiverId, @CreatedAt, @RequestTypeId)",
                new {newRequest.SenderId, newRequest.ReceiverId, newRequest.CreatedAt, newRequest.RequestTypeId});
        }

        public async Task DeleteRequest(Guid userOneId, Guid userTwoId, byte requestTypeId)
        {
            await _dbConnection.ExecuteAsync(
                @"DELETE FROM [dbo].[Requests]
                WHERE ([SenderId] = @UserOneId AND [ReceiverId] = @UserTwoId AND [RequestTypeId] = @RequestTypeId)
                OR ([SenderId] = @UserTwoId AND [ReceiverId] = @UserOneId AND [RequestTypeId] = @RequestTypeId)",
                new {UserOneId = userOneId, UserTwoId = userTwoId, RequestTypeId = requestTypeId});
        }

        public async Task<IList<Request>> GetRequestsByUserIds(Guid userOneId, Guid userTwoId)
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.Request>(
                @"SELECT * FROM [dbo].Requests
                WHERE ([SenderId] = @UserOneId AND [ReceiverId] = @UserTwoId)
                OR ([SenderId] = @UserTwoId AND [ReceiverId] = @UserOneId) ORDER BY [CreatedAt] DESC",
                new {UserOneId = userOneId, UserTwoId = userTwoId}))
                    .Select(req => (Request) req).ToList();
        }

        public async Task<IList<Request>> GetRequestsBySenderId(Guid SenderId)
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.Request>(
                "SELECT * FROM [dbo].Requests WHERE [SenderId] = @SenderId",
                new {SenderId = SenderId})).Select(req => (Request) req).ToList();
        }

        public async Task<Request> GetRequestByUserIdsAndType(Request request)
        {
            return (Request) await _dbConnection.QueryFirstAsync<Models.DTOs.Request>(
                @"SELECT TOP 1 * FROM [dbo].Requests
                WHERE ([SenderId] = @SenderId AND [ReceiverId] = @ReceiverId AND [RequestTypeId] = @RequestTypeId)
                OR ([SenderId] = @ReceiverId AND [ReceiverId] = @SenderId AND [RequestTypeId] = @RequestTypeId)",
                new {SenderId = request.SenderId, ReceiverId = request.ReceiverId, RequestTypeId = request.RequestTypeId});
        }
    }
}