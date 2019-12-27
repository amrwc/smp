using System.ComponentModel.DataAnnotations;

namespace Smp.Web.Models.Requests
{
    public class SignInRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}