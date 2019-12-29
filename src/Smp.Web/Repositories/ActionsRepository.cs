using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;

namespace Smp.Web.Repositories
{
    public interface IActionsRepository
    {
        Task CreateAction(Models.Action action);
        Task<Models.Action> GetActionById(Guid actionId);
        Task CompleteActionById(Guid actionId);
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

        public async Task<Models.Action> GetActionById(Guid actionId)
        {
            var action = await _dbConnection.QuerySingleOrDefaultAsync<Models.DTOs.Action>(
                "SELECT TOP 1 * FROM [Actions] WHERE [Id] = @Id",
                new { Id = actionId }
            );

            return action == null ? null : (Models.Action) action;
        }

        public async Task CompleteActionById(Guid actionId)
        {
            await _dbConnection.ExecuteAsync(
                "UPDATE [Actions] SET [Completed] = 1 WHERE [Id] = @Id",
                new { Id = actionId }
            );
        }
    }
}