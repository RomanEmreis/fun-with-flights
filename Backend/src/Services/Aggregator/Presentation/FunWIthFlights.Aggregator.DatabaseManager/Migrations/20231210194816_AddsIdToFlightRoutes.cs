using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunWIthFlights.Aggregator.DatabaseManager.Migrations
{
    /// <inheritdoc />
    public partial class AddsIdToFlightRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Routes");
        }
    }
}
