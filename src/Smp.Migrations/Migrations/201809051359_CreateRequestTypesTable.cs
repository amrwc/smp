using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809051359, "Create RequestTypes table"), ExcludeFromCodeCoverage]
    public class CreateRequestTypesTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201809051359_CreateRequestTypesTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201809051359_CreateRequestTypesTable.sql"));
        }
    }
}
