using Microsoft.EntityFrameworkCore.Migrations;

namespace shitfo.Migrations
{
    public partial class SettingsEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactMessage",
                table: "Settings",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactTitle",
                table: "Settings",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactMessage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ContactTitle",
                table: "Settings");
        }
    }
}
