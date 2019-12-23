using System;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IRelationshipService
    {
        Task AddFriend(Guid userOneId, Guid userTwoId);
        Task<bool> IsAlreadyFriend(Guid senderId, Guid receiverId);
    }

    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;

        public RelationshipService(IRelationshipRepository friendRepository)
        {
            _relationshipRepository = friendRepository;
        }

        public async Task AddFriend(Guid userOneId, Guid userTwoId)
            => await _relationshipRepository.AddRelationship(userOneId, userTwoId, (await _relationshipRepository.GetRelationshipTypeByName(RelationshipType.Friend)).Id);

        public async Task<bool> IsAlreadyFriend(Guid senderId, Guid receiverId)
        {
            var friend = await _relationshipRepository.GetRelationshipByIdsAndType(senderId, receiverId, (await _relationshipRepository.GetRelationshipTypeByName(RelationshipType.Friend)).Id);

            return friend != null ? true : false;
        }
    }
}