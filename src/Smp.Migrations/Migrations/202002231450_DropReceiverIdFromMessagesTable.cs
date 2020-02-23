using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(202002231450, "Drop ReceiverId Column from Messages Table"), ExcludeFromCodeCoverage]
    public class DropReceiverIdFromMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/202002231450_DropReceiverIdFromMessagesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/202002231450_DropReceiverIdFromMessagesTable.sql"));
        }
    }
}