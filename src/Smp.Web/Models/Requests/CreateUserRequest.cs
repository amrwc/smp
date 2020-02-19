using System.ComponentModel.DataAnnotations;
using Smp.Web.Resources.Constants;

namespace Smp.Web.Models.Requests
{
    public class CreateUserRequest
    {
        [Required, StringLength(32, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string FullName { get; set; }

        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string Password { get; set; }

        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string ConfirmPassword { get; set; }

        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string Email { get; set; }
    }
}
