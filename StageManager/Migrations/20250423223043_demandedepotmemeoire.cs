using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class demandedepotmemeoire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheminFichier",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropColumn(
                name: "Commentaire",
                table: "DemandesDepotMemoire");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Memoires",
                newName: "Titre");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateValidation",
                table: "DemandesDepotMemoire",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EncadreurId",
                table: "DemandesDepotMemoire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomEncadreur",
                table: "DemandesDepotMemoire",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomEtudiants",
                table: "DemandesDepotMemoire",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "themeId",
                table: "DemandesDepotMemoire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDepotMemoire_EncadreurId",
                table: "DemandesDepotMemoire",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDepotMemoire_themeId",
                table: "DemandesDepotMemoire",
                column: "themeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDepotMemoire_Themes_themeId",
                table: "DemandesDepotMemoire",
                column: "themeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_EncadreurId",
                table: "DemandesDepotMemoire",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDepotMemoire_Themes_themeId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_EncadreurId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropIndex(
                name: "IX_DemandesDepotMemoire_EncadreurId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropIndex(
                name: "IX_DemandesDepotMemoire_themeId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropColumn(
                name: "DateValidation",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropColumn(
                name: "EncadreurId",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropColumn(
                name: "NomPrenomEncadreur",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropColumn(
                name: "NomPrenomEtudiants",
                table: "DemandesDepotMemoire");

            migrationBuilder.DropColumn(
                name: "themeId",
                table: "DemandesDepotMemoire");

            migrationBuilder.RenameColumn(
                name: "Titre",
                table: "Memoires",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "CheminFichier",
                table: "DemandesDepotMemoire",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Commentaire",
                table: "DemandesDepotMemoire",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
