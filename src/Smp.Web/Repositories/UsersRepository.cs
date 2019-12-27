using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IUsersRepository
    {
        Task CreateUser(User newUser);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(Guid id);
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsersRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreateUser(User newUser)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO [dbo].[Users] ([Id], [FullName], [Password], [Email], [CreatedAt]) VALUES (@Id, @FullName, @Password, @Email, @CreatedAt)",
                new {newUser.Id, newUser.FullName, newUser.Password, newUser.Email, newUser.CreatedAt});
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var dbUser = await _dbConnection.QueryFirstOrDefaultAsync<Models.DTOs.User>(
                "SELECT TOP 1 * FROM [dbo].[Users] WHERE [Email] = @Email",
                new { Email = email });
            return dbUser == null ? null : (User) dbUser;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var dbUser = await _dbConnection.QueryFirstOrDefaultAsync<Models.DTOs.User>(
                "SELECT TOP 1 * FROM [dbo].[Users] WHERE [Id] = @Id",
                new { Id = id });
            return dbUser == null ? null : (User) dbUser;
        }
    }
}