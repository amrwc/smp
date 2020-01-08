using System.Diagnostics.CodeAnalysis;

namespace Smp.Web.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RelationshipType
    {
        public byte Id { get; set; }
        public string Type { get; set; }
    }
}
