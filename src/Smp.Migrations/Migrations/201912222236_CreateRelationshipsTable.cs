using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222236, "Create Relationships Table")]
    public class CreateRelationshipsTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201912222236_CreateRelationshipsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201912222236_CreateRelationshipsTable.sql"));
        }
    }
}