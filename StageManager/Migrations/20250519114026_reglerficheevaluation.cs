using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class reglerficheevaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstValide",
                table: "FichesEvaluationStagiaire",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EstValide",
                table: "FichesEvaluationEncadreur",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstValide",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "EstValide",
                table: "FichesEvaluationEncadreur");
        }
    }
}
