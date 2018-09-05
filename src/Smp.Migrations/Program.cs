using System;
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
            var connectionString = SelectEnvironmentToMigrate();

            using (var connection = new SqlConnection(connectionString))
            {
                var databaseProvider = new MssqlDatabaseProvider(connection);
                var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider);

                migrator.Load();
                migrator.MigrateToLatest();
            }
        }
            
        private static string SelectEnvironmentToMigrate()
        {
            Console.WriteLine("Select an environment to migrate. Press '1' for local (.), '2' for live, '3' for test live (smp-dev), or '4' for a custom environment.");
            var charPressed = Console.ReadKey().KeyChar;

            switch (charPressed)
            {
                case '1':
                    return new SqlConnectionStringBuilder
                    {
                        DataSource = ".",
                        InitialCatalog = "SMP",
                        IntegratedSecurity = true
                    }.ConnectionString;
                case '2':
                    return new SqlConnectionStringBuilder
                    {
                        DataSource = "smp-db.database.windows.net",
                        InitialCatalog = "SMP",
                        UserID = "SMPAdmin",
                        Password = "|Q6q@8K~Mqwg_lz(lCqUxkz_Pj+i"
                    }.ConnectionString;
                case '3':
                    return new SqlConnectionStringBuilder
                    {
                        DataSource = "smp-db.database.windows.net",
                        InitialCatalog = "smp-dev",
                        UserID = "SMPAdmin",
                        Password = "|Q6q@8K~Mqwg_lz(lCqUxkz_Pj+i"
                    }.ConnectionString;
                case '4':
                    Console.WriteLine("Enter connection string XD");
                    return Console.ReadLine();
                default:
                    return new SqlConnectionStringBuilder
                    {
                        DataSource = ".",
                        InitialCatalog = "SMP",
                        IntegratedSecurity = true
                    }.ConnectionString;
            }
        }
    }
}
