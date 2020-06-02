using Microsoft.EntityFrameworkCore.Migrations;

namespace BubenBot.Data.Migrations
{
    public partial class AddGuildIdToTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "guild_id",
                table: "tags",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guild_id",
                table: "tags");
        }
    }
}
