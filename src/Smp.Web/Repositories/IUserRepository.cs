using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Smp.Web.Repositories
{
    public interface IUserRepository
    {
        void CreateUser(string username, string password, string email);
    }

    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetValue<string>("DatabaseConnectionString"));
            _dbConnection.Open();
        }

        public void CreateUser(string username, string password, string email)
        {
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}