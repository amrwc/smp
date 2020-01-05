using System;
using System.Diagnostics.CodeAnalysis;

namespace Smp.Web.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class Relationship
    {
        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }
        public byte RelationshipTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
