using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netscii.Migrations
{
    /// <inheritdoc />
    public partial class AddedLogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversionActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Format = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessingTimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    OutputLength = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversionParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Characters = table.Column<string>(type: "TEXT", nullable: false),
                    Font = table.Column<string>(type: "TEXT", nullable: false),
                    Background = table.Column<string>(type: "TEXT", nullable: false),
                    Scale = table.Column<int>(type: "INTEGER", nullable: false),
                    Invert = table.Column<bool>(type: "INTEGER", nullable: false),
                    OperatingSystem = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversionAssociations",
                columns: table => new
                {
                    ConversionActivityId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConversionParametersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionAssociations", x => new { x.ConversionActivityId, x.ConversionParametersId });
                    table.ForeignKey(
                        name: "FK_ConversionAssociations_ConversionActivities_ConversionActivityId",
                        column: x => x.ConversionActivityId,
                        principalTable: "ConversionActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversionAssociations_ConversionParameters_ConversionParametersId",
                        column: x => x.ConversionParametersId,
                        principalTable: "ConversionParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionAssociations_ConversionParametersId",
                table: "ConversionAssociations",
                column: "ConversionParametersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversionAssociations");

            migrationBuilder.DropTable(
                name: "ConversionActivities");

            migrationBuilder.DropTable(
                name: "ConversionParameters");
        }
    }
}
