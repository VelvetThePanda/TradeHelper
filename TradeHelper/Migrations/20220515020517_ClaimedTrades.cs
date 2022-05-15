using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHelper.Migrations
{
    public partial class ClaimedTrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Trades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trades");
        }
    }
}
