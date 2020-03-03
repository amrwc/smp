using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222250, "Drop Friends Table"), ExcludeFromCodeCoverage]
    public class DropFriendsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912222250_DropFriendsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912222250_DropFriendsTable", false));
        }
    }
}