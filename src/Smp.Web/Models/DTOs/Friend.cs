using System;

namespace Smp.Web.Models.DTOs
{
    public class Friend
    {
        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
}
