using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class addmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DemandeDeStageId",
                table: "Stagiaires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DemandesDeStage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cheminfichier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesDeStage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_DemandeDeStageId",
                table: "Stagiaires",
                column: "DemandeDeStageId",
                unique: true,
                filter: "[DemandeDeStageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Stagiaires_DemandesDeStage_DemandeDeStageId",
                table: "Stagiaires",
                column: "DemandeDeStageId",
                principalTable: "DemandesDeStage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stagiaires_DemandesDeStage_DemandeDeStageId",
                table: "Stagiaires");

            migrationBuilder.DropTable(
                name: "DemandesDeStage");

            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_DemandeDeStageId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "DemandeDeStageId",
                table: "Stagiaires");
        }
    }
}
