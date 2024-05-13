using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunWithFlights.DataSources.DatabaseManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMockDatasource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Url" },
                values: new object[] { "FlightRouter v1", "http://localhost:5156/flight-routes" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Url" },
                values: new object[] { "FlightRouter", "https://localhost:7001/flight-routes" });
        }
    }
}
