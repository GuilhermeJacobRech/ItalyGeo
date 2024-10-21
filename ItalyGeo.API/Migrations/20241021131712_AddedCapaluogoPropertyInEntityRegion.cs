using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedCapaluogoPropertyInEntityRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CapaluogoComuneId",
                table: "Regions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CapaluogoComuneId",
                table: "Regions",
                column: "CapaluogoComuneId");

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
                name: "FK_Regions_Comunes_CapaluogoComuneId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_CapaluogoComuneId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "CapaluogoComuneId",
                table: "Regions");
        }
    }
}
