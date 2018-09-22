using System;

namespace Smp.Web.Models
{
    public class RequestRequest
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public RequestType RequestTypeId { get; set; }
    }
}
