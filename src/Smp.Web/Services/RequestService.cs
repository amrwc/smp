using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Models.Results;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IRequestService
    {
        Task<List<Error>> ValidateNewRequest(Request request);
        Task<bool> IsFriendRequestAlreadySent(Request request);
        Task<List<Error>> ValidateAcceptRequest (Request request);
        Task AcceptRequest(Request request);
    }

    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IFriendService _friendService;

        public RequestService(IRequestRepository requestRepository, IFriendService friendService)
        {
            _requestRepository = requestRepository;
            _friendService = friendService;
        }

        public async Task<List<Error>> ValidateNewRequest (Request request)
        {
            var errors = new List<Error>();

            if (request.SenderId == request.ReceiverId)
                errors.Add(new Error("invalid_request", "A user cannot add themself as a friend."));
            if (await _friendService.IsAlreadyFriend(request.SenderId, request.ReceiverId)) errors.Add(new Error("invalid_request", "You are already connected."));
            if (await IsFriendRequestAlreadySent(request)) errors.Add(new Error("invalid_request", "The friend request was already sent."));

            return errors;
        }

        public async Task<bool> IsFriendRequestAlreadySent(Request request)
        {
            var requests = await _requestRepository.GetRequestsByUserIds(request.SenderId, request.ReceiverId);

            return requests.Any(req => req.RequestType == RequestType.Friend) ? true : false;
        }

        public async Task<List<Error>> ValidateAcceptRequest (Request request)
        {
            var errors = new List<Error>();

            var requests = await _requestRepository.GetRequestByUserIdsAndType(request);

            if (requests == null) errors.Add(new Error("invalid_request", "There is no request to accept."));

            switch (requests.RequestType)
            {
                case (RequestType.Friend):
                    if (await _friendService.IsAlreadyFriend(request.SenderId, request.ReceiverId)) errors.Add(new Error("invalid_request", "You are already connected."));
                    break;
                default:
                    break;
            }

            return errors;
        }

        public async Task AcceptRequest(Request request)
        {
            switch (request.RequestType)
            {
                case (RequestType.Friend):
                    await _friendService.AddFriend(request.SenderId, request.ReceiverId);
                    await _requestRepository.DeleteRequest(request.SenderId, request.ReceiverId, RequestType.Friend);
                    break;
                default:
                    break;
            }
        }
    }
}