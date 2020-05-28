using Microsoft.EntityFrameworkCore.Migrations;

namespace BubenBot.Data.Migrations
{
    public partial class RenameTagValueToContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Tags",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Tags",
                newName: "Value");
        }
    }
}
