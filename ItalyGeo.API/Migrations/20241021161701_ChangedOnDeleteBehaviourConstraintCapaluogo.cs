using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOnDeleteBehaviourConstraintCapaluogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Comunes_CapaluogoComuneId",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Comunes_CapaluogoComuneId",
                table: "Regions");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Comunes_CapaluogoComuneId",
                table: "Provinces",
                column: "CapaluogoComuneId",
                principalTable: "Comunes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Comunes_CapaluogoComuneId",
                table: "Regions",
                column: "CapaluogoComuneId",
                principalTable: "Comunes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Comunes_CapaluogoComuneId",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Comunes_CapaluogoComuneId",
                table: "Regions");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Comunes_CapaluogoComuneId",
                table: "Provinces",
                column: "CapaluogoComuneId",
                principalTable: "Comunes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Comunes_CapaluogoComuneId",
                table: "Regions",
                column: "CapaluogoComuneId",
                principalTable: "Comunes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
