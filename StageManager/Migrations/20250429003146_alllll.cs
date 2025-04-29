using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class alllll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Domaines_DomaineId",
                table: "Themes");

            migrationBuilder.AddColumn<int>(
                name: "DepartementId",
                table: "Themes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Themes_DepartementId",
                table: "Themes",
                column: "DepartementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Departements_DepartementId",
                table: "Themes",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Domaines_DomaineId",
                table: "Themes",
                column: "DomaineId",
                principalTable: "Domaines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Departements_DepartementId",
                table: "Themes");

            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Domaines_DomaineId",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_Themes_DepartementId",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "DepartementId",
                table: "Themes");

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Domaines_DomaineId",
                table: "Themes",
                column: "DomaineId",
                principalTable: "Domaines",
                principalColumn: "Id");
        }
    }
}
