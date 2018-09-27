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
    public interface IFriendRepository
    {
        Task<Friend> GetFriendByUserIds(Guid userOneId, Guid userTwoId);
        Task AddFriend(Guid userOneId, Guid userTwoId);
    }

    public class FriendRepository : IFriendRepository
    {
        private readonly IDbConnection _dbConnection;

        public FriendRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task<Friend> GetFriendByUserIds(Guid userOneId, Guid userTwoId)
        {
            return (Friend) await _dbConnection.QueryFirstAsync<Models.DTOs.Friend>(
@"SELECT TOP 1 [UserOneId], [UserTwoId] FROM [dbo].[Friends]
WHERE ([UserOneId] = @UserOneId AND [UserTwoId] = @UserTwoId)
OR ([UserOneId] = @UserTwoId AND [UserTwoId] = @UserOneId)",
                new {UserOneId = userOneId, UserTwoId = userTwoId});
        }

        public async Task AddFriend(Guid userOneId, Guid userTwoId)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Friends] ([UserOneId], [UserTwoId], [AcceptedDate]) VALUES (@UserOneId, @UserTwoId, @AcceptedDate)",
                new {UserOneId = userOneId, UserTwoId = userTwoId, AcceptedDate = DateTime.UtcNow});
        }
    }
}