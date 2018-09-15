namespace Smp.Web.Services
{
    public interface ICryptographyService
    {
        string HashAndSaltPassword(string password);
        bool CheckPassword(string password, string hashedPassword);
    }

    public class CryptographyService : ICryptographyService
    {
        public string HashAndSaltPassword(string password) 
            => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

        public bool CheckPassword(string password, string hashedPassword) 
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}