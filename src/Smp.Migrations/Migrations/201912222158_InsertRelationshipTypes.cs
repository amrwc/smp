using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222158, "Insert RelationshipTypes"), ExcludeFromCodeCoverage]
    public class InsertRelationshipTypes : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912222158_InsertRelationshipTypes", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912222158_InsertRelationshipTypes", false));
        }
    }
}