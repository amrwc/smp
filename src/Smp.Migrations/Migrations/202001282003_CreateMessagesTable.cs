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
            Execute(File.ReadAllText(@"../../../Migrations/Up/202001282003_CreateMessagesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/202001282003_CreateMessagesTable.sql"));
        }
    }
}