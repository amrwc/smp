using System;

namespace Smp.Web.Models
{
    public class Relationship
    {
        public Relationship() { }

        public Relationship(Guid userOneId, Guid userTwoId, RelationshipType relationshipType)
        {
            UserOneId = userOneId;
            UserTwoId = userTwoId;
            RelationshipType = relationshipType;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator Relationship(DTOs.Relationship relationship)
        {
            return new Relationship
            {
                UserOneId = relationship.UserOneId,
                UserTwoId = relationship.UserTwoId,
                RelationshipType = (RelationshipType)relationship.RelationshipTypeId,
                CreatedAt = relationship.CreatedAt
            };
        }
    }
}