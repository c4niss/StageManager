using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class updatedemandeaccord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stagiaires",
                table: "DemandesAccord");

            migrationBuilder.AddColumn<int>(
                name: "DemandeaccordId1",
                table: "Stagiaires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_DemandeaccordId1",
                table: "Stagiaires",
                column: "DemandeaccordId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Stagiaires_DemandesAccord_DemandeaccordId1",
                table: "Stagiaires",
                column: "DemandeaccordId1",
                principalTable: "DemandesAccord",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stagiaires_DemandesAccord_DemandeaccordId1",
                table: "Stagiaires");

            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_DemandeaccordId1",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "DemandeaccordId1",
                table: "Stagiaires");

            migrationBuilder.AddColumn<string>(
                name: "Stagiaires",
                table: "DemandesAccord",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
