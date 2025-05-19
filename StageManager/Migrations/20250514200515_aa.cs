using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StagiaireMax",
                table: "Utilisateurs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StagiaireMax",
                table: "Utilisateurs",
                type: "int",
                nullable: true);
        }
    }
}
