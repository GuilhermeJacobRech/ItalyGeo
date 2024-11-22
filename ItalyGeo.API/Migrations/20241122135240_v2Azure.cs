using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class v2Azure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InhabitantName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PatronSaint",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "InhabitantName",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "PatronSaint",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "PublicHoliday",
                table: "Comunes");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InhabitantName",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatronSaint",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InhabitantName",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatronSaint",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicHoliday",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: true);
            }
    }
}
