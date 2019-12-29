using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IActionsRepository
    {
        Task CreateAction(Models.Action action);
    }

    public class ActionsRepository : IActionsRepository
    {
        private readonly IDbConnection _dbConnection;

        public ActionsRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreateAction(Models.Action action)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [Actions] ([Id], [UserId], [ActionTypeId], [Completed], [CreatedAt], [ExpiresAt]) VALUES (@Id, @UserId, @ActionTypeId, @Completed, @CreatedAt, @ExpiresAt)",
                new { Id = action.Id, UserId = action.UserId, ActionTypeId = (byte) action.ActionType, Completed = action.Completed, CreatedAt = action.CreatedAt, ExpiresAt = action.ExpiresAt });
        }
    }
}