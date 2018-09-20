using System;
using System.Data.SqlClient;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Smp.Migrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            var migrationsAssembly = typeof(Program).Assembly;
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "SMP",
                IntegratedSecurity = true
            }.ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var databaseProvider = new MssqlDatabaseProvider(connection);
                var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider);

                migrator.Load();
                migrator.MigrateToLatest();
            }
        }
            

    }
}
