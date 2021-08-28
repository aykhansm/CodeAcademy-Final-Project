using Microsoft.EntityFrameworkCore.Migrations;

namespace shitfo.Migrations
{
    public partial class decimalschangedtoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MonthlyPrice",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DailyPrice",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyPrice",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "DailyPrice",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Area",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
