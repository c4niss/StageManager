using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StagiaireId",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_StagiaireId",
                table: "FichesEvaluationEncadreur",
                column: "StagiaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesEvaluationEncadreur_Utilisateurs_StagiaireId",
                table: "FichesEvaluationEncadreur",
                column: "StagiaireId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichesEvaluationEncadreur_Utilisateurs_StagiaireId",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropIndex(
                name: "IX_FichesEvaluationEncadreur_StagiaireId",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "StagiaireId",
                table: "FichesEvaluationEncadreur");
        }
    }
}
