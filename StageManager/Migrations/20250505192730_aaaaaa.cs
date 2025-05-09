using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class aaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationStagiaire_StageId",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.RenameColumn(
                name: "FicheEvaluationStagiaireId",
                table: "Stages",
                newName: "ficheevaluationencadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_StageId",
                table: "FichesEvaluationStagiaire",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur",
                column: "StageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationStagiaire_StageId",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.RenameColumn(
                name: "ficheevaluationencadreurId",
                table: "Stages",
                newName: "FicheEvaluationStagiaireId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_StageId",
                table: "FichesEvaluationStagiaire",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur",
                column: "StageId");
        }
    }
}
