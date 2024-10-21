using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewProvinceProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "yearCreated",
                table: "Provinces",
                newName: "YearCreated");

            migrationBuilder.AddColumn<float>(
                name: "GDPNominalMlnEuro",
                table: "Provinces",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GDPPerCapitaEuro",
                table: "Provinces",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "Provinces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Zipcode",
                table: "Provinces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GDPNominalMlnEuro",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "GDPPerCapitaEuro",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "Provinces");

            migrationBuilder.RenameColumn(
                name: "YearCreated",
                table: "Provinces",
                newName: "yearCreated");
        }
    }
}
