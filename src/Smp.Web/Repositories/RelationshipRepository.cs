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
    public interface IRelationshipRepository
    {
        Task<Relationship> GetRelationshipByIdsAndType(Guid userOneId, Guid userTwoId, byte relationshipTypeId);
        Task AddRelationship(Guid userOneId, Guid userTwoId, byte relationshipTypeId);
        Task<IList<RelationshipType>> GetRelationshipTypes();
        Task<RelationshipType> GetRelationshipTypeById(byte id);
        Task<RelationshipType> GetRelationshipTypeByName(string name);
    }

    public class RelationshipRepository : IRelationshipRepository
    {
        private readonly IDbConnection _dbConnection;

        public RelationshipRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task<IList<RelationshipType>> GetRelationshipTypes()
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.RelationshipType>(
                @"SELECT * FROM [dbo].[RelationshipTypes] SORT BY [Id] ASC"))
                .Select(relType => (RelationshipType) relType).ToList();
        }

        public async Task<RelationshipType> GetRelationshipTypeById(byte id)
        {
            return (RelationshipType)await _dbConnection.QueryFirstAsync<Models.DTOs.RelationshipType>(
                @"SELECT TOP 1 FROM [dbo].[RelationshipTypes] WHERE [Id] = @Id",
                new { Id = id });
        }

        public async Task<RelationshipType> GetRelationshipTypeByName(string name)
        {
            return (RelationshipType)await _dbConnection.QueryFirstAsync<Models.DTOs.RelationshipType>(
                @"SELECT TOP 1 FROM [dbo].[RelationshipTypes] WHERE [Type] = @RelationshipType",
                new { RelationshipType = name });
        }

        public async Task<Relationship> GetRelationshipByIdsAndType(Guid userOneId, Guid userTwoId, byte relationshipTypeId)
        {
            return (Relationship) await _dbConnection.QueryFirstAsync<Models.DTOs.Relationship>(
                @"SELECT TOP 1 [UserOneId], [UserTwoId] FROM [dbo].[Relationships]
                WHERE (([UserOneId] = @UserOneId AND [UserTwoId] = @UserTwoId)
                OR ([UserOneId] = @UserTwoId AND [UserTwoId] = @UserOneId)) AND [RelationshipTypeId] = @RelationshipTypeId",
                new {UserOneId = userOneId, UserTwoId = userTwoId, RelationshipTypeId = relationshipTypeId});
        }

        public async Task AddRelationship(Guid userOneId, Guid userTwoId, byte relationshipTypeId)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Relationships] ([UserOneId], [UserTwoId], [RelationshipTypeId], [AcceptedDate]) VALUES (@UserOneId, @UserTwoId, @RelationshipTypeId, @AcceptedDate)",
                new {UserOneId = userOneId, UserTwoId = userTwoId, RelationshipTypeId = relationshipTypeId, AcceptedDate = DateTime.UtcNow});
        }
    }
}