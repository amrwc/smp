using System.IO;
using SimpleMigrations;

namespace Smp.Migrations.Migrations
{
    [Migration(201809151600, "Rename Username to FullName")]
    public class RenameUsernameToFullName : Migration
    {
        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201809151600_RenameUsernameToFullName.sql"));
        }

        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201809151600_RenameUsernameToFullName.sql"));
        }
    }
}