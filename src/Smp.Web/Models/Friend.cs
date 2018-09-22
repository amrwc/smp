using System;

namespace Smp.Web.Models
{
    public class Friend
    {
        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }

        public static explicit operator Friend(DTOs.Friend friend)
        {
            return new Friend
            {
                UserOneId = friend.UserOneId,
                UserTwoId = friend.UserTwoId
            };
        }
    }
}