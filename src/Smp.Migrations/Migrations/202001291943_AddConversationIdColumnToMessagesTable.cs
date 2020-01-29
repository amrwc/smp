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
            Execute(File.ReadAllText(@"../../../Migrations/Up/202001291943_AddConversationIdColumnToMessagesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/202001291943_AddConversationIdColumnToMessagesTable.sql"));
        }
    }
}