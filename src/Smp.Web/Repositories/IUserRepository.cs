using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Smp.Web.Models.DTOs;

namespace Smp.Web.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(string username, string password, string email);
    }

    public class UserRepository : IUserRepository
    {
        private IDbConnection _dbConnection;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreateUser(string username, string password, string email) => await _dbConnection.InsertAsync(new User(username, password, email));
    }
}