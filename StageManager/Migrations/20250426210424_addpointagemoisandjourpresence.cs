using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class addpointagemoisandjourpresence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointageMois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mois = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    FicheDePointageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointageMois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointageMois_FichesDePointage_FicheDePointageId",
                        column: x => x.FicheDePointageId,
                        principalTable: "FichesDePointage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jourPresences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jour = table.Column<int>(type: "int", nullable: false),
                    JourSemaine = table.Column<int>(type: "int", nullable: false),
                    EstPresent = table.Column<bool>(type: "bit", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PointageMoisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jourPresences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jourPresences_PointageMois_PointageMoisId",
                        column: x => x.PointageMoisId,
                        principalTable: "PointageMois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_jourPresences_PointageMoisId",
                table: "jourPresences",
                column: "PointageMoisId");

            migrationBuilder.CreateIndex(
                name: "IX_PointageMois_FicheDePointageId",
                table: "PointageMois",
                column: "FicheDePointageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jourPresences");

            migrationBuilder.DropTable(
                name: "PointageMois");
        }
    }
}
