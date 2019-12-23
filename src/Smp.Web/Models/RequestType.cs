namespace Smp.Web.Models
{
    public class RequestType
    {
        public const string Friend = "Friend";

        public byte Id { get; set; }
        public string Type { get; set; }

        public static explicit operator RequestType(DTOs.RequestType requestType)
        {
            return new RequestType
            {
                Id = requestType.Id,
                Type = requestType.Type
            };
        }
    }
}
