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
                "INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [Email]) VALUES (@Id, @Username, @Password, @Email)",
                new {newUser.Id, newUser.Username, newUser.Password, newUser.Email});
        }

    }
}