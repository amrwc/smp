using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261816, "Add CreatedAt Column to Users table"), ExcludeFromCodeCoverage]
    public class AddCreatedAtColumnToUsersTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912261816_AddCreatedAtColumnToUsersTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912261816_AddCreatedAtColumnToUsersTable", false));
        }
    }
}