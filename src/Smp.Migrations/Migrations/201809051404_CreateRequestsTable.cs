using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051404, "Create Requests table"), ExcludeFromCodeCoverage]
    public class CreateRequestsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051404_CreateRequestsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051404_CreateRequestsTable", false));
        }
    }
}
