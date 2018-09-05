using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051414, "Create Comments table")]
    public class CreateCommentsTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201809051414_CreateCommentsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201809051414_CreateCommentsTable.sql"));
        }
    }
}
