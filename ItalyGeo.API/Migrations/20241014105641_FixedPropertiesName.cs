using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class FixedPropertiesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Areakm2",
                table: "Regions",
                newName: "AreaKm2");

            migrationBuilder.RenameColumn(
                name: "AltitudeAboveSea",
                table: "Regions",
                newName: "AltitudeAboveSeaMeterMSL");

            migrationBuilder.RenameColumn(
                name: "AltitudeAboveSea",
                table: "Comunes",
                newName: "AltitudeAboveSeaMeterMSL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AreaKm2",
                table: "Regions",
                newName: "Areakm2");

            migrationBuilder.RenameColumn(
                name: "AltitudeAboveSeaMeterMSL",
                table: "Regions",
                newName: "AltitudeAboveSea");

            migrationBuilder.RenameColumn(
                name: "AltitudeAboveSeaMeterMSL",
                table: "Comunes",
                newName: "AltitudeAboveSea");
        }
    }
}
