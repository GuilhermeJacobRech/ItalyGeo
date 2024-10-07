using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedCapaluogoIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Capaluogo",
                table: "Capaluogo");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Capaluogo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProvinceId",
                table: "Capaluogo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Capaluogo",
                table: "Capaluogo",
                column: "ComuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Capaluogo_ComuneId_ProvinceId_RegionId",
                table: "Capaluogo",
                columns: new[] { "ComuneId", "ProvinceId", "RegionId" },
                unique: true,
                filter: "[ProvinceId] IS NOT NULL AND [RegionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Capaluogo",
                table: "Capaluogo");

            migrationBuilder.DropIndex(
                name: "IX_Capaluogo_ComuneId_ProvinceId_RegionId",
                table: "Capaluogo");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Capaluogo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProvinceId",
                table: "Capaluogo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Capaluogo",
                table: "Capaluogo",
                columns: new[] { "ComuneId", "ProvinceId", "RegionId" });
        }
    }
}
