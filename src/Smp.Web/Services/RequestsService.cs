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
    public interface IRequestsService
    {
        Task<List<Error>> ValidateNewRequest(Request request);
        Task<bool> IsRequestAlreadySent(Request request);
        Task<List<Error>> ValidateAcceptRequest (Request request);
        Task AcceptRequest(Request request);
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

        public async Task<List<Error>> ValidateNewRequest(Request request)
        {
            var errors = new List<Error>();

            if (request.SenderId == request.ReceiverId)
                errors.Add(new Error("invalid_request", "A user cannot add themselves as a friend."));
            if (await _relationshipsService.AreAlreadyFriends(request.SenderId, request.ReceiverId)) errors.Add(new Error("invalid_request", "You are already connected."));
            if (await IsRequestAlreadySent(request)) errors.Add(new Error("invalid_request", "The friend request was already sent."));

            return errors;
        }

        public async Task<bool> IsRequestAlreadySent(Request request)
        {
            var requests = await _requestsRepository.GetRequestsByUserIds(request.SenderId, request.ReceiverId);

            return requests.Any(req => req.RequestTypeId == request.RequestTypeId) ? true : false;
        }

        public async Task<List<Error>> ValidateAcceptRequest(Request request)
        {
            var errors = new List<Error>();

            var req = await _requestsRepository.GetRequestByUserIdsAndType(request);
            var reqType = await _requestsRepository.GetRequestTypeById(request.RequestTypeId);

            if (req == null) errors.Add(new Error("invalid_request", "There is no request to accept."));

            switch (reqType.Type)
            {
                case (RequestType.Friend):
                    if (await _relationshipsService.AreAlreadyFriends(request.SenderId, request.ReceiverId)) errors.Add(new Error("invalid_request", "You are already connected."));
                    break;
                default:
                    break;
            }

            return errors;
        }

        public async Task AcceptRequest(Request request)
        {
            var reqType = await _requestsRepository.GetRequestTypeById(request.RequestTypeId);

            switch (reqType.Type)
            {
                case RequestType.Friend:
                    await _relationshipsService.AddFriend(request.SenderId, request.ReceiverId);
                    await _requestsRepository.DeleteRequest(request.SenderId, request.ReceiverId, request.RequestTypeId);
                    break;
                default:
                    break;
            }
        }
    }
}