using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222236, "Create Relationships Table"), ExcludeFromCodeCoverage]
    public class CreateRelationshipsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912222236_CreateRelationshipsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912222236_CreateRelationshipsTable", false));
        }
    }
}