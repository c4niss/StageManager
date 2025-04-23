using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageManager.Migrations
{
    /// <inheritdoc />
    public partial class ficheevaandpointage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "Commentaire",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FichesDePointage");

            migrationBuilder.RenameColumn(
                name: "HeureEffectuees",
                table: "FichesDePointage",
                newName: "StageId");

            migrationBuilder.RenameColumn(
                name: "DateFin",
                table: "FichesDePointage",
                newName: "DateFinStage");

            migrationBuilder.RenameColumn(
                name: "DateDebut",
                table: "FichesDePointage",
                newName: "DateDebutStage");

            migrationBuilder.AddColumn<int>(
                name: "AdaptationOrganisationMethodesTravail",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnalyseExplicationSynthese",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationConnaissancesNouvelles",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppreciationGlobaleTuteur",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AppreciationRenduTravail",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Aptitudes",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CapaciteApprendreComprendre",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Communication",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompetencesGenerales",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ComprehensionTravaux",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "FichesEvaluationStagiaire",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DisponibiliteMotivationEngagement",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DureeStage",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FonctionEncadreur",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormationStagiaire",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IntegrationSeinService",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MethodeOrganisationTravail",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MissionsConfieesAuStagiaire",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NiveauConnaissances",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomEncadreur",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomEvaluateur",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomStagiaire",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NombreSeancesPrevues",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodeAu",
                table: "FichesEvaluationStagiaire",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodeDu",
                table: "FichesEvaluationStagiaire",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PonctualiteAssiduite",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RealisationMissionsConfiees",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RechercheInformations",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RespectDelaisProcedures",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RigueurSerieux",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StructureAccueil",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThemeStage",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TravailEquipe",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UtilisationMoyensMisDisposition",
                table: "FichesEvaluationStagiaire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccepteExpressionPointsVueDifferents",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommuniqueClairementSimplement",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CritiqueConstructive",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "FichesEvaluationEncadreur",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebutStage",
                table: "FichesEvaluationEncadreur",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinStage",
                table: "FichesEvaluationEncadreur",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DemontreInteretRecherche",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EfficaceGestionSupervision",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EncourageInitiativesEtudiants",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnseigneFaitDemonstrations",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpliqueClairementContenu",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FixeObjectifsClairs",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FonctionEncadreur",
                table: "FichesEvaluationEncadreur",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GereImprevus",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InterrogeEtudiantsFeedback",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InterrogeEtudiantsTravailEffectue",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InviteEtudiantsQuestions",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaintientAttitudeProfessionnelle",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaitriseConnaissances",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MontreImportanceSujetTraite",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomEncadreur",
                table: "FichesEvaluationEncadreur",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomStagiaireEvaluateur",
                table: "FichesEvaluationEncadreur",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "FichesEvaluationEncadreur",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrganiseEtapesRecherche",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrienteEtudiantsRessourcesPertinentes",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PondereQuantiteInformation",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProdigueEncouragementsFeedback",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RencontreRegulierementEtudiants",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RepondQuestionsEtudiants",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransmetDonneesFiables",
                table: "FichesEvaluationEncadreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "FichesDePointage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DonneesPointage",
                table: "FichesDePointage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstValide",
                table: "FichesDePointage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NatureStage",
                table: "FichesDePointage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NomPrenomStagiaire",
                table: "FichesDePointage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomQualitePersonneChargeSuivi",
                table: "FichesDePointage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StructureAccueil",
                table: "FichesDePointage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_FichesDePointage_Stages_StageId",
                table: "FichesDePointage",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichesDePointage_Stages_StageId",
                table: "FichesDePointage");

            migrationBuilder.DropIndex(
                name: "IX_FichesDePointage_StageId",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "AdaptationOrganisationMethodesTravail",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "AnalyseExplicationSynthese",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "ApplicationConnaissancesNouvelles",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "AppreciationGlobaleTuteur",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "AppreciationRenduTravail",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "Aptitudes",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "CapaciteApprendreComprendre",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "Communication",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "CompetencesGenerales",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "ComprehensionTravaux",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "DisponibiliteMotivationEngagement",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "DureeStage",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "FonctionEncadreur",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "FormationStagiaire",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "IntegrationSeinService",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "MethodeOrganisationTravail",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "MissionsConfieesAuStagiaire",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "NiveauConnaissances",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "NomPrenomEncadreur",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "NomPrenomEvaluateur",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "NomPrenomStagiaire",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "NombreSeancesPrevues",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "PeriodeAu",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "PeriodeDu",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "PonctualiteAssiduite",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "RealisationMissionsConfiees",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "RechercheInformations",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "RespectDelaisProcedures",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "RigueurSerieux",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "StructureAccueil",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "ThemeStage",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "TravailEquipe",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "UtilisationMoyensMisDisposition",
                table: "FichesEvaluationStagiaire");

            migrationBuilder.DropColumn(
                name: "AccepteExpressionPointsVueDifferents",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "CommuniqueClairementSimplement",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "CritiqueConstructive",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "DateDebutStage",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "DateFinStage",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "DemontreInteretRecherche",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "EfficaceGestionSupervision",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "EncourageInitiativesEtudiants",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "EnseigneFaitDemonstrations",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "ExpliqueClairementContenu",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "FixeObjectifsClairs",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "FonctionEncadreur",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "GereImprevus",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "InterrogeEtudiantsFeedback",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "InterrogeEtudiantsTravailEffectue",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "InviteEtudiantsQuestions",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "MaintientAttitudeProfessionnelle",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "MaitriseConnaissances",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "MontreImportanceSujetTraite",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "NomPrenomEncadreur",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "NomPrenomStagiaireEvaluateur",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "OrganiseEtapesRecherche",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "OrienteEtudiantsRessourcesPertinentes",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "PondereQuantiteInformation",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "ProdigueEncouragementsFeedback",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "RencontreRegulierementEtudiants",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "RepondQuestionsEtudiants",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "TransmetDonneesFiables",
                table: "FichesEvaluationEncadreur");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "DonneesPointage",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "EstValide",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "NatureStage",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "NomPrenomStagiaire",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "NomQualitePersonneChargeSuivi",
                table: "FichesDePointage");

            migrationBuilder.DropColumn(
                name: "StructureAccueil",
                table: "FichesDePointage");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "FichesDePointage",
                newName: "HeureEffectuees");

            migrationBuilder.RenameColumn(
                name: "DateFinStage",
                table: "FichesDePointage",
                newName: "DateFin");

            migrationBuilder.RenameColumn(
                name: "DateDebutStage",
                table: "FichesDePointage",
                newName: "DateDebut");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FichesEvaluationStagiaire",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Commentaire",
                table: "FichesDePointage",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FichesDePointage",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
