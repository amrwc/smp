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
            SentDate = requestRequest.SentDate;
            RequestTypeId = requestRequest.RequestTypeId;
        }

        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime SentDate { get; set; }
        public RequestType RequestTypeId { get; set; }

        public static explicit operator Request(RequestRequest requestRequest)
        {
            return new Request
            {
                SenderId = requestRequest.SenderId,
                ReceiverId = requestRequest.ReceiverId,
                SentDate = requestRequest.SentDate,
                RequestTypeId = requestRequest.RequestTypeId
            };
        }
    }
}