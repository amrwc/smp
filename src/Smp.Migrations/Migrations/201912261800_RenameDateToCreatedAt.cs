using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261800, "Rename Date To CreatedAt on Comments table"), ExcludeFromCodeCoverage]
    public class RenameDateToCreatedAt : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201912261800_RenameDateToCreatedAt.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201912261800_RenameDateToCreatedAt.sql"));
        }
    }
}