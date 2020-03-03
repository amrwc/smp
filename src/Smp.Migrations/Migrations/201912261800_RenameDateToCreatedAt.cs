using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261800, "Rename Date To CreatedAt on Comments table"), ExcludeFromCodeCoverage]
    public class RenameDateToCreatedAt : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912261800_RenameDateToCreatedAt", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912261800_RenameDateToCreatedAt", false));
        }
    }
}