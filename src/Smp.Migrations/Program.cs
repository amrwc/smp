using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Smp.Migrations
{
    [ExcludeFromCodeCoverage]
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

            using var connection = new SqlConnection(connectionString);
            var databaseProvider = new MssqlDatabaseProvider(connection);
            var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider);

            migrator.Load();
            migrator.MigrateToLatest();
        }
    }
}
