using System;

namespace Smp.Web.Models.Requests
{
    public class CreatePostRequest
    {
        public Guid ReceiverId { get; set; }
        public Guid SenderId { get; set; }
        public string TextContent { get; set; }
    }
}