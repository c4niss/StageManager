using StageManager.Models;

public class FicheEvaluationStagiaireReadDto
{
    public int Id { get; set; }
    public DateTime DateCreation { get; set; }
    public string NomPrenomStagiaire { get; set; }
    public string FormationStagiaire { get; set; }
    public string DureeStage { get; set; }
    public DateTime PeriodeDu { get; set; }
    public DateTime PeriodeAu { get; set; }
    public string StructureAccueil { get; set; }
    public int NombreSeancesPrevues { get; set; }
    public string NomPrenomEncadreur { get; set; }
    public string FonctionEncadreur { get; set; }
    public string ThemeStage { get; set; }
    public string MissionsConfieesAuStagiaire { get; set; }

    // Critères d'évaluation
    public NiveauEvaluationStagiaire RealisationMissionsConfiees { get; set; }
    public NiveauEvaluationStagiaire RespectDelaisProcedures { get; set; }
    public NiveauEvaluationStagiaire ComprehensionTravaux { get; set; }
    public NiveauEvaluationStagiaire AppreciationRenduTravail { get; set; }
    public NiveauEvaluationStagiaire UtilisationMoyensMisDisposition { get; set; }
    public NiveauEvaluationStagiaire NiveauConnaissances { get; set; }
    public NiveauEvaluationStagiaire CompetencesGenerales { get; set; }
    public NiveauEvaluationStagiaire AdaptationOrganisationMethodesTravail { get; set; }
    public NiveauEvaluationStagiaire PonctualiteAssiduite { get; set; }
    public NiveauEvaluationStagiaire RigueurSerieux { get; set; }
    public NiveauEvaluationStagiaire DisponibiliteMotivationEngagement { get; set; }
    public NiveauEvaluationStagiaire IntegrationSeinService { get; set; }
    public NiveauEvaluationStagiaire Aptitudes { get; set; }
    public NiveauEvaluationStagiaire TravailEquipe { get; set; }
    public NiveauEvaluationStagiaire CapaciteApprendreComprendre { get; set; }
    public NiveauEvaluationStagiaire ApplicationConnaissancesNouvelles { get; set; }

    // Sections spécifiques
    public NiveauEvaluationStagiaire RechercheInformations { get; set; }
    public NiveauEvaluationStagiaire MethodeOrganisationTravail { get; set; }
    public NiveauEvaluationStagiaire AnalyseExplicationSynthese { get; set; }
    public NiveauEvaluationStagiaire Communication { get; set; }

    public string AppreciationGlobaleTuteur { get; set; }
    public string Observations { get; set; }
    public string NomPrenomEvaluateur { get; set; }
    public DateTime DateEvaluation { get; set; }

    // Informations liées
    public StagiaireMinimalDto Stagiaire { get; set; }
    public EncadreurMinimalDto Encadreur { get; set; }
    public StageMinimalDto Stage { get; set; }
}