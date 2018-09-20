namespace Smp.Web.Models.Requests
{
    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}