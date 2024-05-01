using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunWithFlights.DataSources.DatabaseManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedMockDatasource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Url" },
                values: new object[] { "Mock Flight Routes Provider", "FlightRouter", "https://localhost:7001/flight-routes" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Url" },
                values: new object[] { "Sample Data Source provider", "Provider 1", "https://zretmlbsszmm4i35zrihcflchm0ktwwj.lambda-url.eu-central-1.on.aws/provider/flights1" });

            migrationBuilder.InsertData(
                table: "DataSources",
                columns: new[] { "Id", "Description", "Name", "Url" },
                values: new object[] { 2, "Sample Data Source provider", "Provider 2", "https://zretmlbsszmm4i35zrihcflchm0ktwwj.lambda-url.eu-central-1.on.aws/provider/flights2" });
        }
    }
}
