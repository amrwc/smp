using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Models.Results;

namespace Smp.Web.Services
{
    public interface IRequestService
    {
        Task<ValidateRequestResult> ValidateRequest(Request request);
    }

    public class RequestService : IRequestService
    {
        public Task<ValidateRequestResult> ValidateRequest(Request request)
        {
            throw new System.NotImplementedException();
        }
    }
}