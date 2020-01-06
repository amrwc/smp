using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051419, "Create Friends table"), ExcludeFromCodeCoverage]
    public class CreateFriendsTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201809051419_CreateFriendsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201809051419_CreateFriendsTable.sql"));
        }
    }
}
