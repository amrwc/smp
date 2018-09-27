using System;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IFriendService
    {
        Task AddFriend(Guid userOneId, Guid userTwoId);
        Task<bool> IsAlreadyFriend(Guid senderId, Guid receiverId);
    }

    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;

        public FriendService(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public async Task AddFriend(Guid userOneId, Guid userTwoId)
            => await _friendRepository.AddFriend(userOneId, userTwoId);

        public async Task<bool> IsAlreadyFriend(Guid senderId, Guid receiverId)
        {
            var friend = await _friendRepository.GetFriendByUserIds(senderId, receiverId);

            return friend != null ? true : false;
        }
    }
}