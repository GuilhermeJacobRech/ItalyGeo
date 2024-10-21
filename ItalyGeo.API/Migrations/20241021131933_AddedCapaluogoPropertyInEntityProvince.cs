using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedCapaluogoPropertyInEntityProvince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Capaluogo");

            migrationBuilder.AddColumn<Guid>(
                name: "CapaluogoComuneId",
                table: "Provinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CapaluogoComuneId",
                table: "Provinces",
                column: "CapaluogoComuneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Comunes_CapaluogoComuneId",
                table: "Provinces",
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

            migrationBuilder.DropIndex(
                name: "IX_Provinces_CapaluogoComuneId",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "CapaluogoComuneId",
                table: "Provinces");

            migrationBuilder.CreateTable(
                name: "Capaluogo",
                columns: table => new
                {
                    ComuneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capaluogo", x => x.ComuneId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capaluogo_ComuneId_ProvinceId_RegionId",
                table: "Capaluogo",
                columns: new[] { "ComuneId", "ProvinceId", "RegionId" },
                unique: true,
                filter: "[ProvinceId] IS NOT NULL AND [RegionId] IS NOT NULL");
        }
    }
}
