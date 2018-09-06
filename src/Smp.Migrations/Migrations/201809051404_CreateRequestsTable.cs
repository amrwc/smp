using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051404, "Create Requests table")]
    public class CreateRequestsTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201809051404_CreateRequestsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201809051404_CreateRequestsTable.sql"));
        }
    }
}
