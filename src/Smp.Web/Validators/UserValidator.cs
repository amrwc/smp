using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Validators
{
    public interface IUserValidator
    {
        IList<Error> ValidateCreateUserRequest(CreateUserRequest createUserRequest);
    }

    public class UserValidator : IUserValidator
    {
        public IList<Error> ValidateCreateUserRequest(CreateUserRequest createUserRequest)
        {
            var errors = new List<Error>();

            if (string.IsNullOrEmpty(createUserRequest.Username)) errors.Add(new Error("invalid_username", "Username cannot be empty."));
            if (string.IsNullOrEmpty(createUserRequest.Password)) errors.Add(new Error("invalid_password", "Password cannot be empty."));
            if (!IsValidPassword(createUserRequest.Password)) errors.Add(new Error("invalid_password", "Password must have at least 8 characters."));
            if (string.IsNullOrEmpty(createUserRequest.Email)) errors.Add(new Error("invalid_email", "Email cannot be empty."));

            if (!IsValidEmail(createUserRequest.Email) && errors.All(error => error.Key != "invalid_email"))
                errors.Add(new Error("invalid_email", "Email must be a valid email address."));

  
            return errors;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailRegex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.None, TimeSpan.FromMilliseconds(100));

                if (emailRegex.Match(email).Success)
                    return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        // TODO: fix
        private bool IsValidPassword(string password) 
            => password.Length > 0 && password.Length > 8;
    }
}