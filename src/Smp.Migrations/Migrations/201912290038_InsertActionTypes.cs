using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912290038, "Insert Action Types"), ExcludeFromCodeCoverage]
    public class InsertActionTypes : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912290038_InsertActionTypes", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912290038_InsertActionTypes", false));
        }
    }
}