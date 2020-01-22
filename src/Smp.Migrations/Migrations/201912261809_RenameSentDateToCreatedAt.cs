using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261809, "Rename SentDate To CreatedAt on Requests table"), ExcludeFromCodeCoverage]
    public class RenameSentDateToCreatedAt : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Up/201912261809_RenameSentDateToCreatedAt.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"../../../Migrations/Down/201912261809_RenameSentDateToCreatedAt.sql"));
        }
    }
}