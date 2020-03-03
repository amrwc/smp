using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051419, "Create Friends table"), ExcludeFromCodeCoverage]
    public class CreateFriendsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051419_CreateFriendsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051419_CreateFriendsTable", false));
        }
    }
}
