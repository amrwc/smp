using System;

namespace Smp.Web.Models.DTOs
{
    public class Relationship
    {
        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }
        public byte RelationshipTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
