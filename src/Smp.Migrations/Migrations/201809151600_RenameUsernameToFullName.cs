using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809151600, "Rename Username to FullName"), ExcludeFromCodeCoverage]
    public class RenameUsernameToFullName : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809151600_RenameUsernameToFullName", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809151600_RenameUsernameToFullName", false));
        }
    }
}