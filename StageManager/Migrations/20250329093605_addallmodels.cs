using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class addallmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttestationId",
                table: "Stagiaires",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FicheDePointageId",
                table: "Stagiaires",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FicheEvaluationStagiaireId",
                table: "Stagiaires",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "Stagiaires",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DemandeaccordId",
                table: "DemandesDeStage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MembreDirectionId",
                table: "DemandesDeStage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MembresDirection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fonction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatePrisePoste = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembresDirection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attestations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateGeneration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheminFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EstDelivree = table.Column<bool>(type: "bit", nullable: false),
                    StagiaireId = table.Column<int>(type: "int", nullable: false),
                    MembreDirectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attestations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attestations_MembresDirection_MembreDirectionId",
                        column: x => x.MembreDirectionId,
                        principalTable: "MembresDirection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attestations_Stagiaires_StagiaireId",
                        column: x => x.StagiaireId,
                        principalTable: "Stagiaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAvenant = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NouvelleFinprevue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motif = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EstAccepter = table.Column<bool>(type: "bit", nullable: false),
                    DateDecision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChefDepartements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartementId = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefDepartements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChefDepartementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departements_ChefDepartements_ChefDepartementId",
                        column: x => x.ChefDepartementId,
                        principalTable: "ChefDepartements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Domaines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domaines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domaines_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Encadreurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fonction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstDisponible = table.Column<bool>(type: "bit", nullable: false),
                    NbrStagiaires = table.Column<int>(type: "int", nullable: false),
                    StagiaireMax = table.Column<int>(type: "int", nullable: false),
                    DepartementId = table.Column<int>(type: "int", nullable: true),
                    DomaineId = table.Column<int>(type: "int", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encadreurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encadreurs_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encadreurs_Domaines_DomaineId",
                        column: x => x.DomaineId,
                        principalTable: "Domaines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DemandesAccord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FichePieceJointe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StagiareId = table.Column<int>(type: "int", nullable: false),
                    StagiaireId = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    DemandeStageId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: true),
                    ChefDepartementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesAccord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandesAccord_ChefDepartements_ChefDepartementId",
                        column: x => x.ChefDepartementId,
                        principalTable: "ChefDepartements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemandesAccord_DemandesDeStage_DemandeStageId",
                        column: x => x.DemandeStageId,
                        principalTable: "DemandesDeStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemandesAccord_Encadreurs_EncadreurId",
                        column: x => x.EncadreurId,
                        principalTable: "Encadreurs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemandesAccord_Stagiaires_StagiaireId",
                        column: x => x.StagiaireId,
                        principalTable: "Stagiaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StagiaireGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    ConventionId = table.Column<int>(type: "int", nullable: false),
                    DepartementId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false),
                    DomaineId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_Domaines_DomaineId",
                        column: x => x.DomaineId,
                        principalTable: "Domaines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stages_Encadreurs_EncadreurId",
                        column: x => x.EncadreurId,
                        principalTable: "Encadreurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conventions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDepot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstValidee = table.Column<bool>(type: "bit", nullable: false),
                    CheminFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    MembreDirectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conventions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conventions_MembresDirection_MembreDirectionId",
                        column: x => x.MembreDirectionId,
                        principalTable: "MembresDirection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conventions_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandesDepotMemoire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    CheminFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    MembreDirectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesDepotMemoire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandesDepotMemoire_MembresDirection_MembreDirectionId",
                        column: x => x.MembreDirectionId,
                        principalTable: "MembresDirection",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemandesDepotMemoire_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FichesDePointage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeureEffectuees = table.Column<int>(type: "int", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StagiaireId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesDePointage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichesDePointage_Encadreurs_EncadreurId",
                        column: x => x.EncadreurId,
                        principalTable: "Encadreurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichesDePointage_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichesDePointage_Stagiaires_StagiaireId",
                        column: x => x.StagiaireId,
                        principalTable: "Stagiaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FichesEvaluationEncadreur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateEvaluation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesEvaluationEncadreur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichesEvaluationEncadreur_Encadreurs_EncadreurId",
                        column: x => x.EncadreurId,
                        principalTable: "Encadreurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichesEvaluationEncadreur_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FichesEvaluationStagiaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateEvaluation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StagiaireId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesEvaluationStagiaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichesEvaluationStagiaire_Encadreurs_EncadreurId",
                        column: x => x.EncadreurId,
                        principalTable: "Encadreurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichesEvaluationStagiaire_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichesEvaluationStagiaire_Stagiaires_StagiaireId",
                        column: x => x.StagiaireId,
                        principalTable: "Stagiaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DemandeaccordId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_DemandesAccord_DemandeaccordId",
                        column: x => x.DemandeaccordId,
                        principalTable: "DemandesAccord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Themes_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memoires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CheminFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateDepot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DemandeDepotMemoireId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memoires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memoires_DemandesDepotMemoire_DemandeDepotMemoireId",
                        column: x => x.DemandeDepotMemoireId,
                        principalTable: "DemandesDepotMemoire",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Memoires_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_StageId",
                table: "Stagiaires",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDeStage_MembreDirectionId",
                table: "DemandesDeStage",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attestations_MembreDirectionId",
                table: "Attestations",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attestations_StagiaireId",
                table: "Attestations",
                column: "StagiaireId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avenants_EncadreurId",
                table: "Avenants",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_Avenants_StageId",
                table: "Avenants",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChefDepartements_DepartementId",
                table: "ChefDepartements",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Conventions_MembreDirectionId",
                table: "Conventions",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Conventions_StageId",
                table: "Conventions",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAccord_ChefDepartementId",
                table: "DemandesAccord",
                column: "ChefDepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAccord_DemandeStageId",
                table: "DemandesAccord",
                column: "DemandeStageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAccord_EncadreurId",
                table: "DemandesAccord",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAccord_StagiaireId",
                table: "DemandesAccord",
                column: "StagiaireId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDepotMemoire_MembreDirectionId",
                table: "DemandesDepotMemoire",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDepotMemoire_StageId",
                table: "DemandesDepotMemoire",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Departements_ChefDepartementId",
                table: "Departements",
                column: "ChefDepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Domaines_DepartementId",
                table: "Domaines",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Encadreurs_DepartementId",
                table: "Encadreurs",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Encadreurs_DomaineId",
                table: "Encadreurs",
                column: "DomaineId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_EncadreurId",
                table: "FichesDePointage",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_StagiaireId",
                table: "FichesDePointage",
                column: "StagiaireId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_EncadreurId",
                table: "FichesEvaluationEncadreur",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationEncadreur_StageId",
                table: "FichesEvaluationEncadreur",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_EncadreurId",
                table: "FichesEvaluationStagiaire",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_StageId",
                table: "FichesEvaluationStagiaire",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_StagiaireId",
                table: "FichesEvaluationStagiaire",
                column: "StagiaireId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memoires_DemandeDepotMemoireId",
                table: "Memoires",
                column: "DemandeDepotMemoireId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memoires_StageId",
                table: "Memoires",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_DepartementId",
                table: "Stages",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_DomaineId",
                table: "Stages",
                column: "DomaineId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_EncadreurId",
                table: "Stages",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_DemandeaccordId",
                table: "Themes",
                column: "DemandeaccordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Themes_StageId",
                table: "Themes",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDeStage_MembresDirection_MembreDirectionId",
                table: "DemandesDeStage",
                column: "MembreDirectionId",
                principalTable: "MembresDirection",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stagiaires_Stages_StageId",
                table: "Stagiaires",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avenants_Encadreurs_EncadreurId",
                table: "Avenants",
                column: "EncadreurId",
                principalTable: "Encadreurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avenants_Stages_StageId",
                table: "Avenants",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChefDepartements_Departements_DepartementId",
                table: "ChefDepartements",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDeStage_MembresDirection_MembreDirectionId",
                table: "DemandesDeStage");

            migrationBuilder.DropForeignKey(
                name: "FK_Stagiaires_Stages_StageId",
                table: "Stagiaires");

            migrationBuilder.DropForeignKey(
                name: "FK_ChefDepartements_Departements_DepartementId",
                table: "ChefDepartements");

            migrationBuilder.DropTable(
                name: "Attestations");

            migrationBuilder.DropTable(
                name: "Avenants");

            migrationBuilder.DropTable(
                name: "Conventions");

            migrationBuilder.DropTable(
                name: "FichesDePointage");

            migrationBuilder.DropTable(
                name: "FichesEvaluationEncadreur");

            migrationBuilder.DropTable(
                name: "FichesEvaluationStagiaire");

            migrationBuilder.DropTable(
                name: "Memoires");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "DemandesDepotMemoire");

            migrationBuilder.DropTable(
                name: "DemandesAccord");

            migrationBuilder.DropTable(
                name: "MembresDirection");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Encadreurs");

            migrationBuilder.DropTable(
                name: "Domaines");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropTable(
                name: "ChefDepartements");

            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_StageId",
                table: "Stagiaires");

            migrationBuilder.DropIndex(
                name: "IX_DemandesDeStage_MembreDirectionId",
                table: "DemandesDeStage");

            migrationBuilder.DropColumn(
                name: "AttestationId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "FicheDePointageId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "FicheEvaluationStagiaireId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "DemandeaccordId",
                table: "DemandesDeStage");

            migrationBuilder.DropColumn(
                name: "MembreDirectionId",
                table: "DemandesDeStage");
        }
    }
}
