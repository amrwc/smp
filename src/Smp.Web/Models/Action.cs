using System;

namespace Smp.Web.Models
{
    public class Action
    {
        public Action() { }

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
        public bool Completed { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public static explicit operator Action(Models.DTOs.Action action)
        {
            return new Action
            {
                Id = action.Id,
                UserId = action.UserId,
                ActionType = (ActionType) action.ActionTypeId,
                Completed = action.Completed,
                CreatedAt = action.CreatedAt,
                ExpiresAt = action.ExpiresAt
            };
        }
    }
}
