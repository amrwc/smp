using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222158, "Insert RelationshipTypes")]
    public class InsertRelationshipTypes : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201912222158_InsertRelationshipTypes.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201912222158_InsertRelationshipTypes.sql"));
        }
    }
}