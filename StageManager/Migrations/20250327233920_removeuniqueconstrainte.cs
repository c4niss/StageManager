using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class removeuniqueconstrainte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_DemandeDeStageId",
                table: "Stagiaires");

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_DemandeDeStageId",
                table: "Stagiaires",
                column: "DemandeDeStageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_DemandeDeStageId",
                table: "Stagiaires");

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_DemandeDeStageId",
                table: "Stagiaires",
                column: "DemandeDeStageId",
                unique: true,
                filter: "[DemandeDeStageId] IS NOT NULL");
        }
    }
}
