using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    status = table.Column<int>(type: "int", nullable: false),
                    CheminFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    DemandeAccordId = table.Column<int>(type: "int", nullable: false),
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
                    StagiaireGroupeId = table.Column<int>(type: "int", nullable: true),
                    ChefDepartementId = table.Column<int>(type: "int", nullable: true),
                    EncadreurId = table.Column<int>(type: "int", nullable: true),
                    ThemeId = table.Column<int>(type: "int", nullable: true),
                    DemandeStageId = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NombreSeancesParSemaine = table.Column<int>(type: "int", nullable: true),
                    DureeSeances = table.Column<int>(type: "int", nullable: true),
                    ServiceAccueil = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UniversiteInstitutEcole = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FiliereSpecialite = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiplomeObtention = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NatureStage = table.Column<int>(type: "int", nullable: true),
                    commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    conventionId = table.Column<int>(type: "int", nullable: false),
                    DateSoumissionStagiaire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RappelJour6Envoye = table.Column<bool>(type: "bit", nullable: false),
                    RappelJour7Envoye = table.Column<bool>(type: "bit", nullable: false)
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
                    themeId = table.Column<int>(type: "int", nullable: false),
                    NomPrenomEtudiants = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomPrenomEncadreur = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateValidation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    MembreDirectionId = table.Column<int>(type: "int", nullable: true),
                    EncadreurId = table.Column<int>(type: "int", nullable: false)
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
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandeaccordId = table.Column<int>(type: "int", nullable: true),
                    MembreDirectionId = table.Column<int>(type: "int", nullable: true),
                    MembreDirectionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesDeStage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandesDeStage_DemandesAccord_DemandeaccordId",
                        column: x => x.DemandeaccordId,
                        principalTable: "DemandesAccord",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FichesDePointage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomPrenomStagiaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StructureAccueil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomQualitePersonneChargeSuivi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebutStage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinStage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NatureStage = table.Column<int>(type: "int", nullable: false),
                    DonneesPointage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstValide = table.Column<bool>(type: "bit", nullable: false),
                    StagiaireId = table.Column<int>(type: "int", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesDePointage", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "FichesEvaluationEncadreur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomPrenomEncadreur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FonctionEncadreur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebutStage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinStage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FixeObjectifsClairs = table.Column<int>(type: "int", nullable: false),
                    GereImprevus = table.Column<int>(type: "int", nullable: false),
                    RencontreRegulierementEtudiants = table.Column<int>(type: "int", nullable: false),
                    OrganiseEtapesRecherche = table.Column<int>(type: "int", nullable: false),
                    ExpliqueClairementContenu = table.Column<int>(type: "int", nullable: false),
                    InterrogeEtudiantsFeedback = table.Column<int>(type: "int", nullable: false),
                    MaitriseConnaissances = table.Column<int>(type: "int", nullable: false),
                    EnseigneFaitDemonstrations = table.Column<int>(type: "int", nullable: false),
                    InviteEtudiantsQuestions = table.Column<int>(type: "int", nullable: false),
                    RepondQuestionsEtudiants = table.Column<int>(type: "int", nullable: false),
                    EncourageInitiativesEtudiants = table.Column<int>(type: "int", nullable: false),
                    InterrogeEtudiantsTravailEffectue = table.Column<int>(type: "int", nullable: false),
                    AccepteExpressionPointsVueDifferents = table.Column<int>(type: "int", nullable: false),
                    CommuniqueClairementSimplement = table.Column<int>(type: "int", nullable: false),
                    CritiqueConstructive = table.Column<int>(type: "int", nullable: false),
                    PondereQuantiteInformation = table.Column<int>(type: "int", nullable: false),
                    EfficaceGestionSupervision = table.Column<int>(type: "int", nullable: false),
                    MaintientAttitudeProfessionnelle = table.Column<int>(type: "int", nullable: false),
                    TransmetDonneesFiables = table.Column<int>(type: "int", nullable: false),
                    OrienteEtudiantsRessourcesPertinentes = table.Column<int>(type: "int", nullable: false),
                    MontreImportanceSujetTraite = table.Column<int>(type: "int", nullable: false),
                    ProdigueEncouragementsFeedback = table.Column<int>(type: "int", nullable: false),
                    DemontreInteretRecherche = table.Column<int>(type: "int", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NomPrenomStagiaireEvaluateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEvaluation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EncadreurId = table.Column<int>(type: "int", nullable: false),
                    StagiaireId = table.Column<int>(type: "int", nullable: false),
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
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomPrenomStagiaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormationStagiaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DureeStage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriodeDu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodeAu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StructureAccueil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreSeancesPrevues = table.Column<int>(type: "int", nullable: false),
                    NomPrenomEncadreur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FonctionEncadreur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThemeStage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissionsConfieesAuStagiaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RealisationMissionsConfiees = table.Column<int>(type: "int", nullable: false),
                    RespectDelaisProcedures = table.Column<int>(type: "int", nullable: false),
                    ComprehensionTravaux = table.Column<int>(type: "int", nullable: false),
                    AppreciationRenduTravail = table.Column<int>(type: "int", nullable: false),
                    UtilisationMoyensMisDisposition = table.Column<int>(type: "int", nullable: false),
                    NiveauConnaissances = table.Column<int>(type: "int", nullable: false),
                    CompetencesGenerales = table.Column<int>(type: "int", nullable: false),
                    AdaptationOrganisationMethodesTravail = table.Column<int>(type: "int", nullable: false),
                    PonctualiteAssiduite = table.Column<int>(type: "int", nullable: false),
                    RigueurSerieux = table.Column<int>(type: "int", nullable: false),
                    DisponibiliteMotivationEngagement = table.Column<int>(type: "int", nullable: false),
                    IntegrationSeinService = table.Column<int>(type: "int", nullable: false),
                    Aptitudes = table.Column<int>(type: "int", nullable: false),
                    TravailEquipe = table.Column<int>(type: "int", nullable: false),
                    CapaciteApprendreComprendre = table.Column<int>(type: "int", nullable: false),
                    ApplicationConnaissancesNouvelles = table.Column<int>(type: "int", nullable: false),
                    RechercheInformations = table.Column<int>(type: "int", nullable: false),
                    MethodeOrganisationTravail = table.Column<int>(type: "int", nullable: false),
                    AnalyseExplicationSynthese = table.Column<int>(type: "int", nullable: false),
                    Communication = table.Column<int>(type: "int", nullable: false),
                    AppreciationGlobaleTuteur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NomPrenomEvaluateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEvaluation = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Titre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
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
                    ConventionId = table.Column<int>(type: "int", nullable: true),
                    DepartementId = table.Column<int>(type: "int", nullable: true),
                    EncadreurId = table.Column<int>(type: "int", nullable: true),
                    DomaineId = table.Column<int>(type: "int", nullable: true),
                    AvenantId = table.Column<int>(type: "int", nullable: false),
                    MemoireId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id");
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
                    StageId = table.Column<int>(type: "int", nullable: true),
                    DepartementId = table.Column<int>(type: "int", nullable: false),
                    DomaineId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Themes_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Themes_Domaines_DomaineId",
                        column: x => x.DomaineId,
                        principalTable: "Domaines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Themes_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
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
                        onDelete: ReferentialAction.SetNull);
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
                name: "IX_Conventions_DemandeAccordId",
                table: "Conventions",
                column: "DemandeAccordId",
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
                name: "IX_DemandesDepotMemoire_EncadreurId",
                table: "DemandesDepotMemoire",
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
                name: "IX_DemandesDepotMemoire_themeId",
                table: "DemandesDepotMemoire",
                column: "themeId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDeStage_DemandeaccordId",
                table: "DemandesDeStage",
                column: "DemandeaccordId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDeStage_MembreDirectionId",
                table: "DemandesDeStage",
                column: "MembreDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesDeStage_MembreDirectionId1",
                table: "DemandesDeStage",
                column: "MembreDirectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Domaines_DepartementId",
                table: "Domaines",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_EncadreurId",
                table: "FichesDePointage",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage",
                column: "StageId");

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
                name: "IX_FichesEvaluationEncadreur_StagiaireId",
                table: "FichesEvaluationEncadreur",
                column: "StagiaireId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_EncadreurId",
                table: "FichesEvaluationStagiaire",
                column: "EncadreurId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_StageId",
                table: "FichesEvaluationStagiaire",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_FichesEvaluationStagiaire_StagiaireId",
                table: "FichesEvaluationStagiaire",
                column: "StagiaireId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_jourPresences_PointageMoisId",
                table: "jourPresences",
                column: "PointageMoisId");

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
                name: "IX_PointageMois_FicheDePointageId",
                table: "PointageMois",
                column: "FicheDePointageId");

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
                name: "IX_Themes_DepartementId",
                table: "Themes",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_DomaineId",
                table: "Themes",
                column: "DomaineId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_StageId",
                table: "Themes",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ChefDepartement_DepartementId",
                table: "Utilisateurs",
                column: "ChefDepartement_DepartementId",
                unique: true,
                filter: "\"TypeUtilisateur\" = 'ChefDepartement'");

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
                name: "FK_Conventions_DemandesAccord_DemandeAccordId",
                table: "Conventions",
                column: "DemandeAccordId",
                principalTable: "DemandesAccord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_FichesDePointage_Stages_StageId",
                table: "FichesDePointage",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_FichesEvaluationEncadreur_Utilisateurs_StagiaireId",
                table: "FichesEvaluationEncadreur",
                column: "StagiaireId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Stages_Utilisateurs_EncadreurId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandesDeStage_DemandesAccord_DemandeaccordId",
                table: "DemandesDeStage");

            migrationBuilder.DropTable(
                name: "Attestations");

            migrationBuilder.DropTable(
                name: "Avenants");

            migrationBuilder.DropTable(
                name: "Conventions");

            migrationBuilder.DropTable(
                name: "FichesEvaluationEncadreur");

            migrationBuilder.DropTable(
                name: "FichesEvaluationStagiaire");

            migrationBuilder.DropTable(
                name: "jourPresences");

            migrationBuilder.DropTable(
                name: "Memoires");

            migrationBuilder.DropTable(
                name: "PointageMois");

            migrationBuilder.DropTable(
                name: "DemandesDepotMemoire");

            migrationBuilder.DropTable(
                name: "FichesDePointage");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Domaines");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropTable(
                name: "DemandesAccord");

            migrationBuilder.DropTable(
                name: "DemandesDeStage");
        }
    }
}
