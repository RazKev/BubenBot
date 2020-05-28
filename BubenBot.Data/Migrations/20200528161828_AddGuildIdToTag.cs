using Microsoft.EntityFrameworkCore.Migrations;

namespace BubenBot.Data.Migrations
{
    public partial class AddGuildIdToTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "Tags",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Tags");
        }
    }
}
