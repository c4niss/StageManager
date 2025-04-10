using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_WithUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });

            migrationBuilder.CreateTable(
                name: "DemandesAccord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    DemandeStageId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreSeancesParSemaine = table.Column<int>(type: "int", nullable: false),
                    DureeSeances = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChefDepartementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesAccord", x => x.Id);
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
                });

            migrationBuilder.CreateTable(
                name: "DemandesDeStage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cheminfichier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    DemandeaccordId = table.Column<int>(type: "int", nullable: true),
                    MembreDirectionId = table.Column<int>(type: "int", nullable: true),
                    MembreDirectionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesDeStage", x => x.Id);
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
                    EncadreurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesDePointage", x => x.Id);
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
                    FicheEvaluationStagiaireId = table.Column<int>(type: "int", nullable: false),
                    AvenantId = table.Column<int>(type: "int", nullable: false),
                    MemoireId = table.Column<int>(type: "int", nullable: false),
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
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TypeUtilisateur = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ChefDepartement_DepartementId = table.Column<int>(type: "int", nullable: true),
                    Encadreur_Fonction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstDisponible = table.Column<bool>(type: "bit", nullable: true),
                    NbrStagiaires = table.Column<int>(type: "int", nullable: true),
                    StagiaireMax = table.Column<int>(type: "int", nullable: true),
                    AvenantId = table.Column<int>(type: "int", nullable: true),
                    DepartementId = table.Column<int>(type: "int", nullable: true),
                    DomaineId = table.Column<int>(type: "int", nullable: true),
                    Fonction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DatePrisePoste = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Universite = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specialite = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    DemandeDeStageId = table.Column<int>(type: "int", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: true),
                    FicheEvaluationStagiaireId = table.Column<int>(type: "int", nullable: true),
                    FicheDePointageId = table.Column<int>(type: "int", nullable: true),
                    AttestationId = table.Column<int>(type: "int", nullable: true),
                    DemandeaccordId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_DemandesAccord_DemandeaccordId",
                        column: x => x.DemandeaccordId,
                        principalTable: "DemandesAccord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_DemandesDeStage_DemandeDeStageId",
                        column: x => x.DemandeDeStageId,
                        principalTable: "DemandesDeStage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Departements_ChefDepartement_DepartementId",
                        column: x => x.ChefDepartement_DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Domaines_DomaineId",
                        column: x => x.DomaineId,
                        principalTable: "Domaines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id");
                });

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
                column: "EncadreurId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avenants_StageId",
                table: "Avenants",
                column: "StageId",
                unique: true);

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
                name: "IX_DemandesDepotMemoire_MembreDirectionId",
                table: "DemandesDepotMemoire",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDepotMemoire_StageId",
                table: "DemandesDepotMemoire",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDeStage_MembreDirectionId",
                table: "DemandesDeStage",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDeStage_MembreDirectionId1",
                table: "DemandesDeStage",
                column: "MembreDirectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Departements_ChefDepartementId",
                table: "Departements",
                column: "ChefDepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Domaines_DepartementId",
                table: "Domaines",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_EncadreurId",
                table: "FichesDePointage",
                column: "EncadreurId");

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
                column: "StageId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_DemandeaccordId",
                table: "Utilisateurs",
                column: "DemandeaccordId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_DemandeDeStageId",
                table: "Utilisateurs",
                column: "DemandeDeStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_DepartementId",
                table: "Utilisateurs",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_DomaineId",
                table: "Utilisateurs",
                column: "DomaineId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_StageId",
                table: "Utilisateurs",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_TypeUtilisateur",
                table: "Utilisateurs",
                column: "TypeUtilisateur");

            migrationBuilder.AddForeignKey(
                name: "FK_Attestations_Utilisateurs_MembreDirectionId",
                table: "Attestations",
                column: "MembreDirectionId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attestations_Utilisateurs_StagiaireId",
                table: "Attestations",
                column: "StagiaireId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avenants_Stages_StageId",
                table: "Avenants",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avenants_Utilisateurs_EncadreurId",
                table: "Avenants",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Conventions_Stages_StageId",
                table: "Conventions",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Conventions_Utilisateurs_MembreDirectionId",
                table: "Conventions",
                column: "MembreDirectionId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_DemandesDeStage_DemandeStageId",
                table: "DemandesAccord",
                column: "DemandeStageId",
                principalTable: "DemandesDeStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord",
                column: "ChefDepartementId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_EncadreurId",
                table: "DemandesAccord",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDepotMemoire_Stages_StageId",
                table: "DemandesDepotMemoire",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDepotMemoire_Utilisateurs_MembreDirectionId",
                table: "DemandesDepotMemoire",
                column: "MembreDirectionId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDeStage_Utilisateurs_MembreDirectionId",
                table: "DemandesDeStage",
                column: "MembreDirectionId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesDeStage_Utilisateurs_MembreDirectionId1",
                table: "DemandesDeStage",
                column: "MembreDirectionId1",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departements_Utilisateurs_ChefDepartementId",
                table: "Departements",
                column: "ChefDepartementId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesDePointage_Utilisateurs_EncadreurId",
                table: "FichesDePointage",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesDePointage_Utilisateurs_StagiaireId",
                table: "FichesDePointage",
                column: "StagiaireId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesEvaluationEncadreur_Stages_StageId",
                table: "FichesEvaluationEncadreur",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FichesEvaluationEncadreur_Utilisateurs_EncadreurId",
                table: "FichesEvaluationEncadreur",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesEvaluationStagiaire_Stages_StageId",
                table: "FichesEvaluationStagiaire",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FichesEvaluationStagiaire_Utilisateurs_EncadreurId",
                table: "FichesEvaluationStagiaire",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesEvaluationStagiaire_Utilisateurs_StagiaireId",
                table: "FichesEvaluationStagiaire",
                column: "StagiaireId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memoires_Stages_StageId",
                table: "Memoires",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Utilisateurs_EncadreurId",
                table: "Stages",
                column: "EncadreurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_ChefDepartementId",
                table: "DemandesAccord");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAccord_Utilisateurs_EncadreurId",
                table: "DemandesAccord");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDeStage_Utilisateurs_MembreDirectionId",
                table: "DemandesDeStage");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDeStage_Utilisateurs_MembreDirectionId1",
                table: "DemandesDeStage");

            migrationBuilder.DropForeignKey(
                name: "FK_Departements_Utilisateurs_ChefDepartementId",
                table: "Departements");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Utilisateurs_EncadreurId",
                table: "Stages");

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
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "DemandesAccord");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "DemandesDeStage");

            migrationBuilder.DropTable(
                name: "Domaines");

            migrationBuilder.DropTable(
                name: "Departements");
        }
    }
}
