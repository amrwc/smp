using System;
using Smp.Web.Models.Requests;

namespace Smp.Web.Models
{
    public class User
    {
        public User(CreateUserRequest user)
        {
            Id = Guid.NewGuid();
            Username = user.Username;
            Password = user.Password;
            Email = user.Email;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}