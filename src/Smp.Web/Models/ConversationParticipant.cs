using System;

namespace Smp.Web.Models
{
    public class ConversationParticipant
    {
        public ConversationParticipant() { }

        public ConversationParticipant(Guid conversationId, Guid userId)
        {
            ConversationId = conversationId;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator ConversationParticipant(DTOs.ConversationParticipant participant)
        {
            return new ConversationParticipant
            {
                ConversationId = participant.ConversationId,
                UserId = participant.UserId,
                CreatedAt = participant.CreatedAt
            };
        }
    }
}
