using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class demandeaccordupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ThemeId",
                table: "DemandesAccord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NombreSeancesParSemaine",
                table: "DemandesAccord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DureeSeances",
                table: "DemandesAccord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFin",
                table: "DemandesAccord",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateDebut",
                table: "DemandesAccord",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "DiplomeObtention",
                table: "DemandesAccord",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DemandesAccord",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FiliereSpecialite",
                table: "DemandesAccord",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NatureStage",
                table: "DemandesAccord",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "DemandesAccord",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "DemandesAccord",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceAccueil",
                table: "DemandesAccord",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "DemandesAccord",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversiteInstitutEcole",
                table: "DemandesAccord",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiplomeObtention",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "FiliereSpecialite",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "NatureStage",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "ServiceAccueil",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "UniversiteInstitutEcole",
                table: "DemandesAccord");

            migrationBuilder.AlterColumn<int>(
                name: "ThemeId",
                table: "DemandesAccord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NombreSeancesParSemaine",
                table: "DemandesAccord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DureeSeances",
                table: "DemandesAccord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFin",
                table: "DemandesAccord",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateDebut",
                table: "DemandesAccord",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
