using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class updatestagemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage");

            migrationBuilder.AddColumn<int>(
                name: "AvenantId",
                table: "Stages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FicheEvaluationStagiaireId",
                table: "Stages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemoireId",
                table: "Stages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage",
                column: "StageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "AvenantId",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "FicheEvaluationStagiaireId",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "MemoireId",
                table: "Stages");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage",
                column: "StageId",
                unique: true);
        }
    }
}
