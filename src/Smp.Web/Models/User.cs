using System;
using Smp.Web.Models.Requests;

namespace Smp.Web.Models
{
    public class User
    {
        public User() {};

        public User(CreateUserRequest user)
        {
            Id = Guid.NewGuid();
            FullName = user.FullName;
            Password = user.Password;
            Email = user.Email;
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public static explicit operator User(Smp.Web.Models.DTOs.User user)
        {
            return new User
            {
                Id = user.Id,
                FullName = user.FullName,
                Password = user.Password,
                Email = user.Email
            };
        }
    }
}