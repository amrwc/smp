using System;

namespace Smp.Web.Models
{
    public class Relationship
    {
        public Relationship() { }

        public Relationship(Guid userOneId, Guid userTwoId, byte relationshipTypeId)
        {
            UserOneId = userOneId;
            UserTwoId = userTwoId;
            RelationshipTypeId = relationshipTypeId;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }
        public byte RelationshipTypeId { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator Relationship(DTOs.Relationship relationship)
        {
            return new Relationship
            {
                UserOneId = relationship.UserOneId,
                UserTwoId = relationship.UserTwoId,
                RelationshipTypeId = relationship.RelationshipTypeId,
                CreatedAt = relationship.CreatedAt
            };
        }
    }
}