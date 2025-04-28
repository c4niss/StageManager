using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class adminaddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId1",
                table: "DemandesAccord");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_EncadreurId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropForeignKey(
                name: "FK_Departements_Utilisateurs_ChefDepartementId",
                table: "Departements");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Departements_ChefDepartement_DepartementId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Departements_ChefDepartementId",
                table: "Departements");

            migrationBuilder.DropIndex(
                name: "IX_DemandesAccord_ChefDepartementId1",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "ChefDepartementId1",
                table: "DemandesAccord");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId",
                unique: true,
                filter: "[DepartementId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord",
                column: "ChefDepartementId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_EncadreurId",
                table: "DemandesDepotMemoire",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Departements_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_EncadreurId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Departements_ChefDepartement_DepartementId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs");

            migrationBuilder.AddColumn<int>(
                name: "ChefDepartementId1",
                table: "DemandesAccord",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Departements_ChefDepartementId",
                table: "Departements",
                column: "ChefDepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAccord_ChefDepartementId1",
                table: "DemandesAccord",
                column: "ChefDepartementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord",
                column: "ChefDepartementId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId1",
                table: "DemandesAccord",
                column: "ChefDepartementId1",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_EncadreurId",
                table: "DemandesDepotMemoire",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departements_Utilisateurs_ChefDepartementId",
                table: "Departements",
                column: "ChefDepartementId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Departements_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
