using System;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IRelationshipsService
    {
        Task AddFriend(Guid userOneId, Guid userTwoId);
        Task<bool> AreAlreadyFriends(Guid senderId, Guid receiverId);
    }

    public class RelationshipsService : IRelationshipsService
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public RelationshipsService(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task AddFriend(Guid userOneId, Guid userTwoId)
        {
            var relationship = new Relationship(userOneId, userTwoId, RelationshipType.Friend);

            await _relationshipsRepository.AddRelationship(relationship);
        }

        public async Task<bool> AreAlreadyFriends(Guid senderId, Guid receiverId)
        {
            var friend = await _relationshipsRepository.GetRelationshipByIdsAndType(senderId, receiverId, RelationshipType.Friend);

            return friend != null;
        }
    }
}