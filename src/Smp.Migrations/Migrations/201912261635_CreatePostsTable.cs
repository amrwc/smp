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
            Execute(MigrationUtils.ReadMigration("201912261635_CreatePostsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912261635_CreatePostsTable", false));
        }
    }
}