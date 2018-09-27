using System;
using Smp.Web.Models.DTOs;
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
            SentDate = DateTime.UtcNow;
            RequestType = requestRequest.RequestType;
        }

        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime SentDate { get; set; }
        public RequestType RequestType { get; set; }

        public static explicit operator Request(DTOs.Request request)
        {
            return new Request
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                SentDate = request.SentDate,
                RequestType = (RequestType) request.RequestTypeId
            };
        }
    }
}