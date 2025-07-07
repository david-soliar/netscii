using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netscii.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNamesLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperatingSystem",
                table: "ConversionParameters",
                newName: "Platform");

            migrationBuilder.RenameColumn(
                name: "OutputLength",
                table: "ConversionActivities",
                newName: "OutputLengthBytes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Platform",
                table: "ConversionParameters",
                newName: "OperatingSystem");

            migrationBuilder.RenameColumn(
                name: "OutputLengthBytes",
                table: "ConversionActivities",
                newName: "OutputLength");
        }
    }
}
