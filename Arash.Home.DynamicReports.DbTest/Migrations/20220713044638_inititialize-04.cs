using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arash.Home.DynamicReports.DbTest.Migrations
{
    public partial class inititialize04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Post",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Category",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Category");
        }
    }
}
