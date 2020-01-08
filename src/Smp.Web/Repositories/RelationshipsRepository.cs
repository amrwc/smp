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
    public interface IRelationshipsRepository
    {
        Task<Relationship> GetRelationshipByIdsAndType(Guid userOneId, Guid userTwoId, RelationshipType relationshipType);
        Task AddRelationship(Relationship relationship);
        Task<IList<RelationshipType>> GetRelationshipTypes();
        Task<RelationshipType> GetRelationshipTypeById(byte id);
        Task<RelationshipType> GetRelationshipTypeByName(string name);
    }

    [ExcludeFromCodeCoverage]
    public class RelationshipsRepository : IRelationshipsRepository
    {
        private readonly IDbConnection _dbConnection;

        public RelationshipsRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task<IList<RelationshipType>> GetRelationshipTypes()
        {
            return (await _dbConnection.QueryAsync<Models.DTOs.RelationshipType>(
                @"SELECT * FROM [dbo].[RelationshipTypes] SORT BY [Id] ASC"))
                .Select(relType => (RelationshipType)relType.Id).ToList();
        }

        public async Task<RelationshipType> GetRelationshipTypeById(byte id)
        {
            return (RelationshipType)(await _dbConnection.QueryFirstAsync<Models.DTOs.RelationshipType>(
                @"SELECT TOP 1 * FROM [dbo].[RelationshipTypes] WHERE [Id] = @Id",
                new { Id = id })).Id;
        }

        public async Task<RelationshipType> GetRelationshipTypeByName(string name)
        {
            return (RelationshipType)(await _dbConnection.QueryFirstAsync<Models.DTOs.RelationshipType>(
                @"SELECT TOP 1 * FROM [dbo].[RelationshipTypes] WHERE [Type] = @RelationshipType",
                new { RelationshipType = name })).Id;
        }

        public async Task<Relationship> GetRelationshipByIdsAndType(Guid userOneId, Guid userTwoId, RelationshipType relationshipType)
        {
            var relationship = await _dbConnection.QueryFirstOrDefaultAsync<Models.DTOs.Relationship>(
                @"SELECT TOP 1 * FROM [dbo].[Relationships]
                WHERE (([UserOneId] = @UserOneId AND [UserTwoId] = @UserTwoId)
                OR ([UserOneId] = @UserTwoId AND [UserTwoId] = @UserOneId)) AND [RelationshipTypeId] = @RelationshipTypeId",
                new {UserOneId = userOneId, UserTwoId = userTwoId, RelationshipTypeId = (byte)relationshipType});

            return relationship == null ? null : (Relationship) relationship;
        }

        public async Task AddRelationship(Relationship relationship)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Relationships] ([UserOneId], [UserTwoId], [RelationshipTypeId], [CreatedAt]) VALUES (@UserOneId, @UserTwoId, @RelationshipTypeId, @CreatedAt)",
                new {UserOneId = relationship.UserOneId, UserTwoId = relationship.UserTwoId, RelationshipTypeId = (byte)relationship.RelationshipType, CreatedAt = relationship.CreatedAt});
        }
    }
}