using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace shitfo.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Category_CategoryId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImage_Properties_PropertyId",
                table: "PropertyImage");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTag_Properties_PropertyId",
                table: "PropertyTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTag_Tag_TagId",
                table: "PropertyTag");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorite_AspNetUsers_AppUserId",
                table: "UserFavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorite_Properties_PropertyId",
                table: "UserFavorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavorite",
                table: "UserFavorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTag",
                table: "PropertyTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImage",
                table: "PropertyImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "UserFavorite",
                newName: "UserFavorites");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "PropertyTag",
                newName: "PropertyTags");

            migrationBuilder.RenameTable(
                name: "PropertyImage",
                newName: "PropertyImages");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorite_PropertyId",
                table: "UserFavorites",
                newName: "IX_UserFavorites_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorite_AppUserId",
                table: "UserFavorites",
                newName: "IX_UserFavorites_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTag_TagId",
                table: "PropertyTags",
                newName: "IX_PropertyTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTag_PropertyId",
                table: "PropertyTags",
                newName: "IX_PropertyTags_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyImage_PropertyId",
                table: "PropertyImages",
                newName: "IX_PropertyImages_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTags",
                table: "PropertyTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImages",
                table: "PropertyImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    PropertyId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingStart = table.Column<DateTime>(nullable: false),
                    BookingEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppUserId",
                table: "Bookings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PropertyId",
                table: "Bookings",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Categories_CategoryId",
                table: "Properties",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImages_Properties_PropertyId",
                table: "PropertyImages",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTags_Properties_PropertyId",
                table: "PropertyTags",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTags_Tags_TagId",
                table: "PropertyTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_AspNetUsers_AppUserId",
                table: "UserFavorites",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Properties_PropertyId",
                table: "UserFavorites",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Categories_CategoryId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImages_Properties_PropertyId",
                table: "PropertyImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTags_Properties_PropertyId",
                table: "PropertyTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTags_Tags_TagId",
                table: "PropertyTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_AspNetUsers_AppUserId",
                table: "UserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Properties_PropertyId",
                table: "UserFavorites");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTags",
                table: "PropertyTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImages",
                table: "PropertyImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "UserFavorites",
                newName: "UserFavorite");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "PropertyTags",
                newName: "PropertyTag");

            migrationBuilder.RenameTable(
                name: "PropertyImages",
                newName: "PropertyImage");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorites_PropertyId",
                table: "UserFavorite",
                newName: "IX_UserFavorite_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorites_AppUserId",
                table: "UserFavorite",
                newName: "IX_UserFavorite_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTags_TagId",
                table: "PropertyTag",
                newName: "IX_PropertyTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTags_PropertyId",
                table: "PropertyTag",
                newName: "IX_PropertyTag_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertyImage",
                newName: "IX_PropertyImage_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavorite",
                table: "UserFavorite",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTag",
                table: "PropertyTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImage",
                table: "PropertyImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Category_CategoryId",
                table: "Properties",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImage_Properties_PropertyId",
                table: "PropertyImage",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTag_Properties_PropertyId",
                table: "PropertyTag",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTag_Tag_TagId",
                table: "PropertyTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorite_AspNetUsers_AppUserId",
                table: "UserFavorite",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorite_Properties_PropertyId",
                table: "UserFavorite",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
