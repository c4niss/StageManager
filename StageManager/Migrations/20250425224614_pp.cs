using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class pp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord");

            migrationBuilder.AddColumn<int>(
                name: "ChefDepartementId1",
                table: "DemandesAccord",
                type: "int",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId1",
                table: "DemandesAccord");

            migrationBuilder.DropIndex(
                name: "IX_DemandesAccord_ChefDepartementId1",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "ChefDepartementId1",
                table: "DemandesAccord");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord",
                column: "ChefDepartementId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");
        }
    }
}
