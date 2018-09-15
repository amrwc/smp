namespace Smp.Web.Services
{
    public interface IAuthService
    {
        bool VerifyUser(string email, string password);
    }

    public class AuthService : IAuthService
    {


        public bool VerifyUser(string email, string password)
        {
            return true;
        }
    }
}