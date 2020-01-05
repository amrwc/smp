using System.Collections.Generic;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Validators
{
    public interface IRequestValidator
    {
        Task<IList<Error>> ValidateNewRequest(Request request);
        Task<IList<Error>> ValidateAcceptRequest(Request request);
    }

    public class RequestValidator : IRequestValidator
    {
        private readonly IRequestsRepository _requestsRepository;
        private readonly IRelationshipsService _relationshipsService;
        private readonly IRequestsService _requestsService;

        public RequestValidator(IRequestsRepository requestsRepository, IRelationshipsService relationshipsService, IRequestsService requestsService)
        {
            _requestsRepository = requestsRepository;
            _relationshipsService = relationshipsService;
            _requestsService = requestsService;
        }

        public async Task<IList<Error>> ValidateNewRequest(Request request)
        {
            var errors = new List<Error>();

            if (request.SenderId == request.ReceiverId)
                errors.Add(new Error("invalid_request", "A user cannot send themselves a request."));
            if (request.RequestType == RequestType.Friend && await _relationshipsService.AreAlreadyFriends(request.SenderId, request.ReceiverId)) errors.Add(new Error("invalid_request", "You are already connected."));
            if (await _requestsService.IsRequestAlreadySent(request)) errors.Add(new Error("invalid_request", "The request has already been sent."));

            return errors;
        }

        public async Task<IList<Error>> ValidateAcceptRequest(Request request)
        {
            var errors = new List<Error>();

            var req = await _requestsRepository.GetRequestByUserIdsAndType(request);
            if (req == null) errors.Add(new Error("invalid_request", "There is no request to accept."));

            switch (request.RequestType)
            {
                case RequestType.Friend:
                    if (await _relationshipsService.AreAlreadyFriends(request.SenderId, request.ReceiverId)) errors.Add(new Error("invalid_request", "You are already connected."));
                    break;
                case RequestType.None:
                    break;
                default:
                    break;
            }

            return errors;
        }
    }
}