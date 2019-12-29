using System;

namespace Smp.Web.Models
{
    public class Action
    {
        public Action(Guid userId, ActionType actionType)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            ActionType = actionType;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = CreatedAt.AddMinutes(15);
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
