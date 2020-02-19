using System.ComponentModel.DataAnnotations;
using Smp.Web.Resources.Constants;

namespace Smp.Web.Models.Requests
{
    public class SignInRequest
    {
        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string Email { get; set; }

        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string Password { get; set; }
    }
}
