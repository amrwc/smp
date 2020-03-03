using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051525, "Insert RequestTypes"), ExcludeFromCodeCoverage]
    public class InsertRequestTypes : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051525_InsertRequestTypes", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051525_InsertRequestTypes", false));
        }
    }
}
