using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222145, "Create Friends table"), ExcludeFromCodeCoverage]
    public class CreateRelationshipTypesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912222145_CreateRelationshipTypesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912222145_CreateRelationshipTypesTable", false));
        }
    }
}