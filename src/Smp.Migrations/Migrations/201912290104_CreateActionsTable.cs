using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912290104, "Create Actions Table"), ExcludeFromCodeCoverage]
    public class CreateActionsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912290104_CreateActionsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912290104_CreateActionsTable", false));
        }
    }
}