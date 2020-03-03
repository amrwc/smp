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
            Execute(MigrationUtils.ReadMigration("201912261801_RenameAcceptedDateToCreatedAt", true));
        }

        protected override void Down()
        {
            Execute(MigrationUtils.ReadMigration("201912261801_RenameAcceptedDateToCreatedAt", false));
        }
    }
}