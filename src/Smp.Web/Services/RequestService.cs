using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Models.Results;

namespace Smp.Web.Services
{
    public interface IRequestService
    {
        IList<Error> ValidateRequest(Request request);
    }

    public class RequestService : IRequestService
    {
        public IList<Error> ValidateRequest (Request request)
        {
            var errors = new List<Error>();

            if (request.SenderId == request.ReceiverId)
                errors.Add(new Error("invalid_request", "A user cannot add themself as a friend."));

            return errors;
        }
    }
}