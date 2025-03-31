using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class addrelationbetweenstagiaireanddeamndeaccord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Stagiaires_StagiaireId",
                table: "DemandesAccord");

            migrationBuilder.DropIndex(
                name: "IX_DemandesAccord_StagiaireId",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "StagiaireId",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "StagiareId",
                table: "DemandesAccord");

            migrationBuilder.AlterColumn<int>(
                name: "FicheDePointageId",
                table: "Stagiaires",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AttestationId",
                table: "Stagiaires",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DemandeaccordId",
                table: "Stagiaires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_DemandeaccordId",
                table: "Stagiaires",
                column: "DemandeaccordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stagiaires_DemandesAccord_DemandeaccordId",
                table: "Stagiaires",
                column: "DemandeaccordId",
                principalTable: "DemandesAccord",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stagiaires_DemandesAccord_DemandeaccordId",
                table: "Stagiaires");

            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_DemandeaccordId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "DemandeaccordId",
                table: "Stagiaires");

            migrationBuilder.AlterColumn<int>(
                name: "FicheDePointageId",
                table: "Stagiaires",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttestationId",
                table: "Stagiaires",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StagiaireId",
                table: "DemandesAccord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StagiareId",
                table: "DemandesAccord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAccord_StagiaireId",
                table: "DemandesAccord",
                column: "StagiaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Stagiaires_StagiaireId",
                table: "DemandesAccord",
                column: "StagiaireId",
                principalTable: "Stagiaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
