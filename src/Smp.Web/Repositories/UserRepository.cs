using System.Data;
using System;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models.DTOs;

namespace Smp.Web.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(Models.User newUser);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        // public async Task CreateUser(Models.User newUser) 
        //     => await _dbConnection.InsertAsync(new User(newUser.Id, newUser.Username, newUser.Password, newUser.Email));

        public async Task CreateUser(Models.User newUser) 
        {
            await _dbConnection.ExecuteAsync("@INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [Email]) VALUES (@Id, @Username, @Password, @Email)", new { Id = newUser.Id, Username = newUser.Username, Password = newUser.Password, Email = newUser.Email });
        }

    }
}