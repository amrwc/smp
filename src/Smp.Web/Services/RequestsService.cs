using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IRequestsService
    {
        Task<bool> IsRequestAlreadySent(Request request);
        Task AcceptRequest(Request request);
        Task DeclineRequest(Request request);
    }

    public class RequestsService : IRequestsService
    {
        private readonly IRequestsRepository _requestsRepository;
        private readonly IRelationshipsService _relationshipsService;

        public RequestsService(IRequestsRepository requestsRepository, IRelationshipsService relationshipsService)
        {
            _requestsRepository = requestsRepository;
            _relationshipsService = relationshipsService;
        }
        
        public async Task<bool> IsRequestAlreadySent(Request request)
            => await _requestsRepository.GetRequestByUserIdsAndType(request) != null;

        public async Task AcceptRequest(Request request)
        {
            switch (request.RequestType)
            {
                case RequestType.Friend:
                    await _relationshipsService.AddFriend(request.SenderId, request.ReceiverId);
                    await _requestsRepository.DeleteRequest(request.SenderId, request.ReceiverId, request.RequestType);
                    break;
            }
        }

        public async Task DeclineRequest(Request request)
            => await _requestsRepository.DeleteRequest(request.SenderId, request.ReceiverId, request.RequestType);
    }
}