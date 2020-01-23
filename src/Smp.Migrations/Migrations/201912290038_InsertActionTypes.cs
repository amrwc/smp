using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912290038, "Insert Action Types"), ExcludeFromCodeCoverage]
    public class InsertActionTypes : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201912290038_InsertActionTypes.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201912290038_InsertActionTypes.sql"));
        }
    }
}