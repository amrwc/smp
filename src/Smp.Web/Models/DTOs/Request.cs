using System;

namespace Smp.Web.Models.DTOs
{
    public class Request
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte RequestTypeId { get; set; }
    }
}
