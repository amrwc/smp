using System;
using Smp.Web.Models.Requests;

namespace Smp.Web.Models
{
    public class Message
    {
        public Message() {}

        public Message(CreateMessageRequest message)
        {
            ReceiverId = message.ReceiverId;
            SenderId = message.SenderId;
            Content = message.Content;
            CreatedAt = DateTime.UtcNow;
            ConversationId = message.ConversationId;
        }

        public long Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public Guid ConversationId { get; set; }

        public static explicit operator Message(DTOs.Message message)
        {
            return new Message
            {
                Id = message.Id,
                ReceiverId = message.ReceiverId,
                SenderId = message.SenderId,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
                ConversationId = message.ConversationId
            };
        }
    }
}
