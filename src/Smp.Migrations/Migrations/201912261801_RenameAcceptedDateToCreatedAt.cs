using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261801, "Rename AcceptedDate To CreatedAt on Relationships table"), ExcludeFromCodeCoverage]
    public class RenameAcceptedDateToCreatedAt : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201912261801_RenameAcceptedDateToCreatedAt.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201912261801_RenameAcceptedDateToCreatedAt.sql"));
        }
    }
}