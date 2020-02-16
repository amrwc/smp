using System;

namespace Smp.Web.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator Conversation(DTOs.Conversation conversation)
        {
            return new Conversation
            {
                Id = conversation.Id,
                CreatedAt = conversation.CreatedAt
            };
        }
    }
}
