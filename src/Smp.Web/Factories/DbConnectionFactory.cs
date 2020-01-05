using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Smp.Web.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetDbConnection();
    }

    [ExcludeFromCodeCoverage]
    public class DbConnectionFactory : IDbConnectionFactory, IDisposable
    {
        private readonly IDbConnection _dbConnection;
        public DbConnectionFactory(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetValue<string>("DatabaseConnectionString"));
            _dbConnection.Open();
        }

        public void Dispose()
            => _dbConnection?.Dispose();

        public IDbConnection GetDbConnection() 
            => _dbConnection;
    }
}