using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202001291943, "Add ConversationId Column to Messages Table"), ExcludeFromCodeCoverage]
    public class AddConversationIdColumnToMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("202001291943_AddConversationIdColumnToMessagesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("202001291943_AddConversationIdColumnToMessagesTable", false));
        }
    }
}