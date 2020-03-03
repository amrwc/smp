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
            Execute(MigrationUtils.ReadMigration("202002231450_DropReceiverIdFromMessagesTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("202002231450_DropReceiverIdFromMessagesTable", false));
        }
    }
}