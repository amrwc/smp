using System.ComponentModel.DataAnnotations;

namespace Smp.Web.Models.Requests
{
    public class CreateUserRequest
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }
    }
}