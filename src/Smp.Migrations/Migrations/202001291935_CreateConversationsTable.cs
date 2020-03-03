using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202001291935, "Create Conversations Table"), ExcludeFromCodeCoverage]
    public class CreateConversationsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("202001291935_CreateConversationsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("202001291935_CreateConversationsTable", false));
        }
    }
}