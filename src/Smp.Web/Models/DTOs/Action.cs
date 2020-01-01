using System;

namespace Smp.Web.Models.DTOs
{
    public class Action
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte ActionTypeId { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
