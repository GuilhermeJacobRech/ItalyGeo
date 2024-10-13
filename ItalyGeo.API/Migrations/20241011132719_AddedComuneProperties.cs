using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedComuneProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AltitudeAboveSea",
                table: "Comunes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AreaKm2",
                table: "Comunes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "InhabitantName",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "InhabitantsPerKm2",
                table: "Comunes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "PatronSaint",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Population",
                table: "Comunes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PublicHoliday",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Comunes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltitudeAboveSea",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "AreaKm2",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "InhabitantName",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "InhabitantsPerKm2",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "PatronSaint",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "Population",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "PublicHoliday",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "Comunes");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Comunes");
        }
    }
}
