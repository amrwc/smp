using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051359, "Create RequestTypes table"), ExcludeFromCodeCoverage]
    public class CreateRequestTypesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051359_CreateRequestTypesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051359_CreateRequestTypesTable", false));
        }
    }
}
