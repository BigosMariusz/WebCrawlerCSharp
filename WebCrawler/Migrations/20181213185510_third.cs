using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentUrl",
                table: "Links");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Links",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Links");

            migrationBuilder.AddColumn<string>(
                name: "ParentUrl",
                table: "Links",
                nullable: true);
        }
    }
}
