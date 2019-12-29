using System;
using System.ComponentModel.DataAnnotations;

namespace Smp.Web.Models.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public Guid ActionId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}