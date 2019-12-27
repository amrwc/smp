using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201912261816, "Add CreatedAt Column to Users table")]
    public class AddCreatedAtColumnToUsersTable : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201912261816_AddCreatedAtColumnToUsersTable.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201912261816_AddCreatedAtColumnToUsersTable.sql"));
        }
    }
}