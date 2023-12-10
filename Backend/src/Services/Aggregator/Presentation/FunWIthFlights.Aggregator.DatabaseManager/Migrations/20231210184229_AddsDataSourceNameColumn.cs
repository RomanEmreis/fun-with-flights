using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunWIthFlights.Aggregator.DatabaseManager.Migrations
{
    /// <inheritdoc />
    public partial class AddsDataSourceNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataSourceName",
                table: "Routes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSourceName",
                table: "Routes");
        }
    }
}
