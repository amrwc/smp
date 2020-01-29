using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202001291938, "Create ConversationParticipants Table"), ExcludeFromCodeCoverage]
    public class CreateConversationParticipantsTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/202001291938_CreateConversationParticipantsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/202001291938_CreateConversationParticipantsTable.sql"));
        }
    }
}