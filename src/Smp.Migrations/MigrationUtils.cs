using System;
using System.IO;
using System.Reflection;

namespace Smp.Migrations
{
    internal class MigrationUtils
    {
        public static string ReadMigration(string migrationName, bool upMigration)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream($"Smp.Migrations.Migrations.{(upMigration ? "Up" : "Down" )}.{migrationName}.sql");

            if (stream == null) throw new Exception("Embedded resource could not be found.");

            return new StreamReader(stream).ReadToEnd();
        }
    }
}
