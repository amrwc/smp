using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261829, "Rename Date To CreatedAt in Messages table"), ExcludeFromCodeCoverage]
    public class RenameDateToCreatedAtInMessagesTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201912261829_RenameDateToCreatedAt.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201912261829_RenameDateToCreatedAt.sql"));
        }
    }
}