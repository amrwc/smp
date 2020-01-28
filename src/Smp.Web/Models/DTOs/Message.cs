using System;
using System.Diagnostics.CodeAnalysis;

namespace Smp.Web.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class Message
    {
        public long Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
    }
}
