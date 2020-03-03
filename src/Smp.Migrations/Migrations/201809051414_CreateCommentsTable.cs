using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051414, "Create Comments table"), ExcludeFromCodeCoverage]
    public class CreateCommentsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051414_CreateCommentsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051414_CreateCommentsTable", false));
        }
    }
}
