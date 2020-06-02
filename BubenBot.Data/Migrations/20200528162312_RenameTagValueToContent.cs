using Microsoft.EntityFrameworkCore.Migrations;

namespace BubenBot.Data.Migrations
{
    public partial class RenameTagValueToContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "value",
                table: "tags",
                newName: "content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "content",
                table: "tags",
                newName: "value");
        }
    }
}
