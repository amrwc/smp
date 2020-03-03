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
            Execute(MigrationUtils.ReadMigration("201912261809_RenameSentDateToCreatedAt", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912261809_RenameSentDateToCreatedAt", false));
        }
    }
}