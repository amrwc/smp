using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202001282002, "Drop Messages Table"), ExcludeFromCodeCoverage]
    public class DropMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("202001282002_DropMessagesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("202001282002_DropMessagesTable", false));
        }
    }
}