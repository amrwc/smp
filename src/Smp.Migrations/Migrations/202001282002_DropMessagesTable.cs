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
            Execute(File.ReadAllText(@"../../../Migrations/Up/202001282002_DropMessagesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/202001282002_DropMessagesTable.sql"));
        }
    }
}