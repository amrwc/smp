﻿using System;

namespace Smp.Web.Models.DTOs
{
    public class User
    {
        public User(Guid id, string username, string password, string email)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
