using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunWIthFlights.Aggregator.DatabaseManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Airline = table.Column<string>(type: "text", nullable: false),
                    SourceAirport = table.Column<string>(type: "text", nullable: false),
                    DestinationAirport = table.Column<string>(type: "text", nullable: false),
                    CodeShare = table.Column<string>(type: "text", nullable: false),
                    Stops = table.Column<int>(type: "integer", nullable: false),
                    Equipment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SourceAirport_DestinationAirport",
                table: "Routes",
                columns: new[] { "SourceAirport", "DestinationAirport" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
