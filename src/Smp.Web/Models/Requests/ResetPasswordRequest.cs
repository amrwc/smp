using System;
using System.ComponentModel.DataAnnotations;
using Smp.Web.Resources.Constants;

namespace Smp.Web.Models.Requests
{
    public class ResetPasswordRequest
    {
        [Required] public Guid ActionId { get; set; }

        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string NewPassword { get; set; }

        [Required, StringLength(255, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string ConfirmNewPassword { get; set; }
    }
}
