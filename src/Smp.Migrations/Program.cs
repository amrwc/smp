using System.Data.SqlClient;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Smp.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var migrationsAssembly = typeof(Program).Assembly;
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "SMP",
                IntegratedSecurity = true
            };

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                var databaseProvider = new MssqlDatabaseProvider(connection);
                var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider);

                migrator.Load();
                migrator.MigrateToLatest();
            }
        }
    }
}
