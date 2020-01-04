using System;
using Smp.Web.Models.Requests;

namespace Smp.Web.Models
{
    public class Request
    {
        public Request() {}

        public Request(RequestRequest requestRequest)
        {
            SenderId = requestRequest.SenderId;
            ReceiverId = requestRequest.ReceiverId;
            CreatedAt = DateTime.UtcNow;
            RequestType = (RequestType)requestRequest.RequestTypeId;
        }

        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public RequestType RequestType { get; set; }

        public static explicit operator Request(DTOs.Request request)
        {
            return new Request
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                CreatedAt = request.CreatedAt,
                RequestType = (RequestType)request.RequestTypeId
            };
        }
    }
}