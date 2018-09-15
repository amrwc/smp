using System.Data;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(User newUser);
        Task<User> GetUser(string Email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreateUser(User newUser)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Users] ([Id], [FullName], [Password], [Email]) VALUES (@Id, @FullName, @Password, @Email)",
                new {newUser.Id, newUser.FullName, newUser.Password, newUser.Email});
        }

        public async Task<User> GetUser(string Email)
        {
            return new (Smp.Web.Models.User) new Smp.Web.Models.DTOs.User();
        }
    }
}