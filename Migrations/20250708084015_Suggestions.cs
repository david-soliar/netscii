using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netscii.Migrations
{
    /// <inheritdoc />
    public partial class Suggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversionAssociations");

            migrationBuilder.AddColumn<int>(
                name: "ConversionParametersId",
                table: "ConversionActivities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SuggestionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuggestionCategoryAssociations",
                columns: table => new
                {
                    SuggestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SuggestionCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestionCategoryAssociations", x => new { x.SuggestionId, x.SuggestionCategoryId });
                    table.ForeignKey(
                        name: "FK_SuggestionCategoryAssociations_SuggestionCategories_SuggestionCategoryId",
                        column: x => x.SuggestionCategoryId,
                        principalTable: "SuggestionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuggestionCategoryAssociations_Suggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "Suggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionActivities_ConversionParametersId",
                table: "ConversionActivities",
                column: "ConversionParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionCategoryAssociations_SuggestionCategoryId",
                table: "SuggestionCategoryAssociations",
                column: "SuggestionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionActivities_ConversionParameters_ConversionParametersId",
                table: "ConversionActivities",
                column: "ConversionParametersId",
                principalTable: "ConversionParameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionActivities_ConversionParameters_ConversionParametersId",
                table: "ConversionActivities");

            migrationBuilder.DropTable(
                name: "SuggestionCategoryAssociations");

            migrationBuilder.DropTable(
                name: "SuggestionCategories");

            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_ConversionActivities_ConversionParametersId",
                table: "ConversionActivities");

            migrationBuilder.DropColumn(
                name: "ConversionParametersId",
                table: "ConversionActivities");

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
    }
}
