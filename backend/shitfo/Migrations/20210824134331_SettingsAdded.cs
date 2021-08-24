using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace shitfo.Migrations
{
    public partial class SettingsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Logo = table.Column<string>(maxLength: 150, nullable: true),
                    ContactImage = table.Column<string>(maxLength: 150, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 100, nullable: true),
                    ContactEmailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    ContactAddress = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
