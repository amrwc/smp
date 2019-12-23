using System;

namespace Smp.Web.Models
{
    public class Relationship
    {
        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }
        public byte RelationshipTypeId { get; set; }

        public static explicit operator Relationship(DTOs.Relationship friend)
        {
            return new Relationship
            {
                UserOneId = friend.UserOneId,
                UserTwoId = friend.UserTwoId,
                RelationshipTypeId = friend.RelationshipTypeId
            };
        }
    }
}