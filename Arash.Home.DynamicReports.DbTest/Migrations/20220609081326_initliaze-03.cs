using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arash.Home.DynamicReports.DbTest.Migrations
{
    public partial class initliaze03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_CategoryModel_CategoryId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryModel",
                table: "CategoryModel");

            migrationBuilder.RenameTable(
                name: "CategoryModel",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "Title");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Category_CategoryId",
                table: "Post",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Category_CategoryId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "CategoryModel");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "CategoryModel",
                newName: "Name");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryModel",
                table: "CategoryModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_CategoryModel_CategoryId",
                table: "Post",
                column: "CategoryId",
                principalTable: "CategoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
