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
            Execute(File.ReadAllText(@"../../../Migrations/Up/202001291935_CreateConversationsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/202001291935_CreateConversationsTable.sql"));
        }
    }
}