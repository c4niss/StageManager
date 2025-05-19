using StageManager.DTOs;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class FicheEvaluationStagiaireMapping
    {
        // Conversion de FicheEvaluationStagiaire vers FicheEvaluationStagiaireReadDto
        public static FicheEvaluationStagiaireReadDto ToReadDto(this FicheEvaluationStagiaire fiche)
        {
            return new FicheEvaluationStagiaireReadDto
            {
                Id = fiche.Id,
                DateCreation = fiche.DateCreation,
                NomPrenomStagiaire = fiche.NomPrenomStagiaire,
                FormationStagiaire = fiche.FormationStagiaire,
                DureeStage = fiche.DureeStage,
                PeriodeDu = fiche.PeriodeDu,
                PeriodeAu = fiche.PeriodeAu,
                StructureAccueil = fiche.StructureAccueil,
                NombreSeancesPrevues = fiche.NombreSeancesPrevues,
                NomPrenomEncadreur = fiche.NomPrenomEncadreur,
                FonctionEncadreur = fiche.FonctionEncadreur,
                ThemeStage = fiche.ThemeStage,
                MissionsConfieesAuStagiaire = fiche.MissionsConfieesAuStagiaire,

                // Critères d'évaluation
                RealisationMissionsConfiees = fiche.RealisationMissionsConfiees,
                RespectDelaisProcedures = fiche.RespectDelaisProcedures,
                ComprehensionTravaux = fiche.ComprehensionTravaux,
                AppreciationRenduTravail = fiche.AppreciationRenduTravail,
                UtilisationMoyensMisDisposition = fiche.UtilisationMoyensMisDisposition,
                NiveauConnaissances = fiche.NiveauConnaissances,
                CompetencesGenerales = fiche.CompetencesGenerales,
                AdaptationOrganisationMethodesTravail = fiche.AdaptationOrganisationMethodesTravail,
                PonctualiteAssiduite = fiche.PonctualiteAssiduite,
                RigueurSerieux = fiche.RigueurSerieux,
                DisponibiliteMotivationEngagement = fiche.DisponibiliteMotivationEngagement,
                IntegrationSeinService = fiche.IntegrationSeinService,
                Aptitudes = fiche.Aptitudes,
                TravailEquipe = fiche.TravailEquipe,
                CapaciteApprendreComprendre = fiche.CapaciteApprendreComprendre,
                ApplicationConnaissancesNouvelles = fiche.ApplicationConnaissancesNouvelles,

                // Sections spécifiques
                RechercheInformations = fiche.RechercheInformations,
                MethodeOrganisationTravail = fiche.MethodeOrganisationTravail,
                AnalyseExplicationSynthese = fiche.AnalyseExplicationSynthese,
                Communication = fiche.Communication,

                AppreciationGlobaleTuteur = fiche.AppreciationGlobaleTuteur,
                Observations = fiche.Observations,
                NomPrenomEvaluateur = fiche.NomPrenomEvaluateur,
                DateEvaluation = fiche.DateEvaluation,

                // Informations liées
                Stagiaire = fiche.Stagiaire != null ? new StagiaireMinimalDto
                {
                    Id = fiche.Stagiaire.Id,
                    NomPrenom = $"{fiche.Stagiaire.Nom} {fiche.Stagiaire.Prenom}"
                } : null,
                Encadreur = fiche.Encadreur != null ? new EncadreurMinimalDto
                {
                    Id = fiche.Encadreur.Id,
                    NomPrenom = $"{fiche.Encadreur.Nom} {fiche.Encadreur.Prenom}"
                } : null,
                Stage = fiche.Stage != null ? new StageMinimalDto
                {
                    Id = fiche.Stage.Id,
                } : null
            };
        }

        // Conversion de FicheEvaluationStagiaireCreateDto vers FicheEvaluationStagiaire
        public static FicheEvaluationStagiaire ToEntity(this FicheEvaluationStagiaireCreateDto dto)
        {
            return new FicheEvaluationStagiaire
            {
                DateCreation = DateTime.Now,
                NomPrenomStagiaire = dto.NomPrenomStagiaire,
                FormationStagiaire = dto.FormationStagiaire,
                DureeStage = dto.DureeStage,
                PeriodeDu = dto.PeriodeDu,
                PeriodeAu = dto.PeriodeAu,
                StructureAccueil = dto.StructureAccueil,
                NombreSeancesPrevues = dto.NombreSeancesPrevues,
                NomPrenomEncadreur = dto.NomPrenomEncadreur,
                FonctionEncadreur = dto.FonctionEncadreur,
                ThemeStage = dto.ThemeStage,
                MissionsConfieesAuStagiaire = dto.MissionsConfieesAuStagiaire,

                // Critères d'évaluation
                RealisationMissionsConfiees = dto.RealisationMissionsConfiees,
                RespectDelaisProcedures = dto.RespectDelaisProcedures,
                ComprehensionTravaux = dto.ComprehensionTravaux,
                AppreciationRenduTravail = dto.AppreciationRenduTravail,
                UtilisationMoyensMisDisposition = dto.UtilisationMoyensMisDisposition,
                NiveauConnaissances = dto.NiveauConnaissances,
                CompetencesGenerales = dto.CompetencesGenerales,
                AdaptationOrganisationMethodesTravail = dto.AdaptationOrganisationMethodesTravail,
                PonctualiteAssiduite = dto.PonctualiteAssiduite,
                RigueurSerieux = dto.RigueurSerieux,
                DisponibiliteMotivationEngagement = dto.DisponibiliteMotivationEngagement,
                IntegrationSeinService = dto.IntegrationSeinService,
                Aptitudes = dto.Aptitudes,
                TravailEquipe = dto.TravailEquipe,
                CapaciteApprendreComprendre = dto.CapaciteApprendreComprendre,
                ApplicationConnaissancesNouvelles = dto.ApplicationConnaissancesNouvelles,

                // Sections spécifiques
                RechercheInformations = dto.RechercheInformations,
                MethodeOrganisationTravail = dto.MethodeOrganisationTravail,
                AnalyseExplicationSynthese = dto.AnalyseExplicationSynthese,
                Communication = dto.Communication,

                AppreciationGlobaleTuteur = dto.AppreciationGlobaleTuteur,
                Observations = dto.Observations,
                NomPrenomEvaluateur = dto.NomPrenomEvaluateur,

                StagiaireId = dto.StagiaireId,
                EncadreurId = dto.EncadreurId,
                StageId = dto.StageId,
                EstValide = false // Par défaut, la fiche n'est pas validée
            };
        }

        // Mise à jour de FicheEvaluationStagiaire à partir de FicheEvaluationStagiaireUpdateDto
        public static void UpdateFromDto(this FicheEvaluationStagiaire fiche, FicheEvaluationStagiaireUpdateDto dto)
        {
            fiche.MissionsConfieesAuStagiaire = dto.MissionsConfieesAuStagiaire;

            // Critères d'évaluation
            fiche.RealisationMissionsConfiees = dto.RealisationMissionsConfiees;
            fiche.RespectDelaisProcedures = dto.RespectDelaisProcedures;
            fiche.ComprehensionTravaux = dto.ComprehensionTravaux;
            fiche.AppreciationRenduTravail = dto.AppreciationRenduTravail;
            fiche.UtilisationMoyensMisDisposition = dto.UtilisationMoyensMisDisposition;
            fiche.NiveauConnaissances = dto.NiveauConnaissances;
            fiche.CompetencesGenerales = dto.CompetencesGenerales;
            fiche.AdaptationOrganisationMethodesTravail = dto.AdaptationOrganisationMethodesTravail;
            fiche.PonctualiteAssiduite = dto.PonctualiteAssiduite;
            fiche.RigueurSerieux = dto.RigueurSerieux;
            fiche.DisponibiliteMotivationEngagement = dto.DisponibiliteMotivationEngagement;
            fiche.IntegrationSeinService = dto.IntegrationSeinService;
            fiche.Aptitudes = dto.Aptitudes;
            fiche.TravailEquipe = dto.TravailEquipe;
            fiche.CapaciteApprendreComprendre = dto.CapaciteApprendreComprendre;
            fiche.ApplicationConnaissancesNouvelles = dto.ApplicationConnaissancesNouvelles;

            // Sections spécifiques
            fiche.RechercheInformations = dto.RechercheInformations;
            fiche.MethodeOrganisationTravail = dto.MethodeOrganisationTravail;
            fiche.AnalyseExplicationSynthese = dto.AnalyseExplicationSynthese;
            fiche.Communication = dto.Communication;

            fiche.AppreciationGlobaleTuteur = dto.AppreciationGlobaleTuteur;
            fiche.Observations = dto.Observations;
            fiche.NomPrenomEvaluateur = dto.NomPrenomEvaluateur;
        }
    }
}