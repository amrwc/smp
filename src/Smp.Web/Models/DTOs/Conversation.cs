using System;
using System.Diagnostics.CodeAnalysis;

namespace Smp.Web.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class Conversation
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
