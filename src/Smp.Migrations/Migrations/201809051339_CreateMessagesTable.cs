using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051339, "Create Messages table"), ExcludeFromCodeCoverage]
    public class CreateMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051339_CreateMessagesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051339_CreateMessagesTable", false));
        }
    }
}
