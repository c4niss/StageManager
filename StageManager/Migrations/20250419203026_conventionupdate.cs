using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class conventionupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conventions_DemandeAccordId",
                table: "Conventions");

            migrationBuilder.AddColumn<int>(
                name: "conventionId",
                table: "DemandesAccord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Conventions_DemandeAccordId",
                table: "Conventions",
                column: "DemandeAccordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conventions_DemandeAccordId",
                table: "Conventions");

            migrationBuilder.DropColumn(
                name: "conventionId",
                table: "DemandesAccord");

            migrationBuilder.CreateIndex(
                name: "IX_Conventions_DemandeAccordId",
                table: "Conventions",
                column: "DemandeAccordId");
        }
    }
}
