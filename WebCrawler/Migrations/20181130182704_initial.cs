using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    ParentUrl = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true),
                    IsLocal = table.Column<bool>(nullable: false),
                    Tier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
