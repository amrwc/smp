using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809050840, "Create Users table"), ExcludeFromCodeCoverage]
    public class CreateUsersTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809050840_CreateUsersTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809050840_CreateUsersTable", false));
        }
    }
}
