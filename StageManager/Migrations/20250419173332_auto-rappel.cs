using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class autorappel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateSoumissionStagiaire",
                table: "DemandesAccord",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "RappelJour6Envoye",
                table: "DemandesAccord",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RappelJour7Envoye",
                table: "DemandesAccord",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSoumissionStagiaire",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "RappelJour6Envoye",
                table: "DemandesAccord");

            migrationBuilder.DropColumn(
                name: "RappelJour7Envoye",
                table: "DemandesAccord");
        }
    }
}
