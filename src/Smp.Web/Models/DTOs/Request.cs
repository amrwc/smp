using System;

namespace Smp.Web.Models.DTOs
{
    public class Request
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime SentDate { get; set; }
        public short RequestTypeId { get; set; }
    }
}
