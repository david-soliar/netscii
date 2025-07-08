using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netscii.Migrations
{
    /// <inheritdoc />
    public partial class SuggestionsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionActivities_ConversionParameters_ConversionParametersId",
                table: "ConversionActivities");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionActivities_ConversionParameters_ConversionParametersId",
                table: "ConversionActivities",
                column: "ConversionParametersId",
                principalTable: "ConversionParameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionActivities_ConversionParameters_ConversionParametersId",
                table: "ConversionActivities");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionActivities_ConversionParameters_ConversionParametersId",
                table: "ConversionActivities",
                column: "ConversionParametersId",
                principalTable: "ConversionParameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
