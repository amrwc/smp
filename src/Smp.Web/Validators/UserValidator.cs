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

            if (!IsValidFullName(createUserRequest?.FullName)) errors.Add(new Error("invalid_full_name", "Full name must have at least 3 characters."));
            if (!IsValidPassword(createUserRequest?.Password)) errors.Add(new Error("invalid_password", "Password must have at least 8 characters, at least 1 lowercase letter, at least 1 uppercase letter, a number, and a symbol."));
            if (!IsValidEmail(createUserRequest?.Email)) errors.Add(new Error("invalid_email", "Email must be a valid email address."));

            return errors;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

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

        private bool IsValidFullName(string fullName)
            => !string.IsNullOrEmpty(fullName) && fullName.Length >= 3;

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            try
            {
                var passwordRegex = new Regex(@"^(?=\P{Ll}*\p{Ll})(?=\P{Lu}*\p{Lu})(?=\P{N}*\p{N})(?=[\p{L}\p{N}]*[^\p{L}\p{N}])[\s\S]{8,}$", RegexOptions.None, TimeSpan.FromMilliseconds(100));

                if (passwordRegex.Match(password).Success) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}