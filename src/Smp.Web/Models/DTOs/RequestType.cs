using System.Diagnostics.CodeAnalysis;

namespace Smp.Web.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RequestType
    {
        public byte Id { get; set; }
        public string Type { get; set; }
    }
}
