namespace Smp.Web.Models
{
    public class RelationshipType
    {
        public const string Friend = "Friend";

        public byte Id { get; set; }
        public string Type { get; set; }

        public static explicit operator RelationshipType(DTOs.RelationshipType relationshipType)
        {
            return new RelationshipType
            {
                Id = relationshipType.Id,
                Type = relationshipType.Type
            };
        }
    }
}