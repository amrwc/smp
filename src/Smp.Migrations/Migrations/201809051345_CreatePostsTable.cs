using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051345, "Create Posts table"), ExcludeFromCodeCoverage]
    public class CreatePostsTable : Migration
    {
        protected override void Up()
        {
            Execute(MigrationUtils.ReadMigration("201809051345_CreatePostsTable", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201809051345_CreatePostsTable", false));
        }
    }
}
