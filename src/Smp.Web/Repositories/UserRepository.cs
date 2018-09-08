using System.Data;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
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

        public async Task CreateUser(Models.User newUser) 
            => await _dbConnection.InsertAsync(new User(newUser.Username, newUser.Password, newUser.Email));
    }
}