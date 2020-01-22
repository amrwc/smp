using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051339, "Create Messages table"), ExcludeFromCodeCoverage]
    public class CreateMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201809051339_CreateMessagesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201809051339_CreateMessagesTable.sql"));
        }
    }
}
