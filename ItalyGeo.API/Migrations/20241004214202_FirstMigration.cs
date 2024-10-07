using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItalyGeo.API.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Capaluogo",
                columns: table => new
                {
                    ComuneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capaluogo", x => new { x.ComuneId, x.ProvinceId, x.RegionId });
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    WikipediaPagePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: false),
                    Areakm2 = table.Column<float>(type: "real", nullable: false),
                    InhabitantsPerKm2 = table.Column<float>(type: "real", nullable: false),
                    ComuneCount = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    WikipediaPagePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Acronym = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: false),
                    Areakm2 = table.Column<float>(type: "real", nullable: false),
                    InhabitantsPerKm2 = table.Column<float>(type: "real", nullable: false),
                    ComuneCount = table.Column<long>(type: "bigint", nullable: false),
                    yearCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comunes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    WikipediaPagePath = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comunes_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comunes_ProvinceId",
                table: "Comunes",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comunes_WikipediaPagePath",
                table: "Comunes",
                column: "WikipediaPagePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_RegionId",
                table: "Provinces",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_WikipediaPagePath",
                table: "Provinces",
                column: "WikipediaPagePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_WikipediaPagePath",
                table: "Regions",
                column: "WikipediaPagePath",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Capaluogo");

            migrationBuilder.DropTable(
                name: "Comunes");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
