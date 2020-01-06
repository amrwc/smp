using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261635, "Create Posts Table with new schema"), ExcludeFromCodeCoverage]
    public class CreatePostsTableNewSchema : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201912261635_CreatePostsTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201912261635_CreatePostsTable.sql"));
        }
    }
}