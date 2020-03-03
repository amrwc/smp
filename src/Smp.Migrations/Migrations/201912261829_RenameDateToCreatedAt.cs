using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261829, "Rename Date To CreatedAt in Messages table"), ExcludeFromCodeCoverage]
    public class RenameDateToCreatedAtInMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201912261829_RenameDateToCreatedAt", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912261829_RenameDateToCreatedAt", false));
        }
    }
}