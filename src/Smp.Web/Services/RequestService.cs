using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Models.Results;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IRequestService
    {
        Task<List<Error>> ValidateRequest(Request request);
    }

    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<List<Error>> ValidateRequest (Request request)
        {
            var errors = new List<Error>();

            if (request.SenderId == request.ReceiverId)
                errors.Add(new Error("invalid_request", "A user cannot add themself as a friend."));

            if (await IsAlreadyFriend(request)) errors.Add(new Error("invalid_request", "You are already connected."));

            return errors;
        }

        public async Task<bool> IsAlreadyFriend(Request request)
        {
            var friend = await _requestRepository.GetFriendByUserIds(request.SenderId, request.ReceiverId);

            return friend != null ? true : false;
        }
    }
}