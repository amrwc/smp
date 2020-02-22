namespace Smp.Web.Models.Results
{
    public class VerifyUserResult
    {
        public VerifyUserResult(bool success, User user)
        {
            Success = success;
            User = user;
        }
        public bool Success { get; }
        public User User { get; }
    }
}