using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class aaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId",
                unique: true,
                filter: "\"TypeUtilisateur\" = 'ChefDepartement'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId",
                unique: true,
                filter: "[DepartementId] IS NOT NULL");
        }
    }
}
