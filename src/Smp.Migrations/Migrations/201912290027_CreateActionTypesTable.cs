using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912290027, "Create ActionTypes Table"), ExcludeFromCodeCoverage]
    public class CreateActionTypesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912290027_CreateActionTypesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912290027_CreateActionTypesTable", false));
        }
    }
}