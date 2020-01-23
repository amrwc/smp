using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222250, "Drop Friends Table"), ExcludeFromCodeCoverage]
    public class DropFriendsTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201912222250_DropFriendsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201912222250_DropFriendsTable.sql"));
        }
    }
}