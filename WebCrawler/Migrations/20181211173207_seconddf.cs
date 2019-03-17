using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.Migrations
{
    public partial class seconddf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsLocal",
                table: "Links",
                newName: "IsInternal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsInternal",
                table: "Links",
                newName: "IsLocal");
        }
    }
}
