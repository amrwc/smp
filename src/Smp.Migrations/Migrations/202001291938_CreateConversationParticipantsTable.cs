using System.Diagnostics.CodeAnalysis;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202001291938, "Create ConversationParticipants Table"), ExcludeFromCodeCoverage]
    public class CreateConversationParticipantsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("202001291938_CreateConversationParticipantsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("202001291938_CreateConversationParticipantsTable", false));
        }
    }
}