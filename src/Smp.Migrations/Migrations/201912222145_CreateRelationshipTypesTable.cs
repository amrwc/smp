using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912222145, "Create Friends table"), ExcludeFromCodeCoverage]
    public class CreateRelationshipTypesTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201912222145_CreateRelationshipTypesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201912222145_CreateRelationshipTypesTable.sql"));
        }
    }
}