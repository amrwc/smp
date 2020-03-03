using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202001282003, "Create Messages Table"), ExcludeFromCodeCoverage]
    public class CreateMessagesTableWithIdentity : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("202001282003_CreateMessagesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("202001282003_CreateMessagesTable", false));
        }
    }
}