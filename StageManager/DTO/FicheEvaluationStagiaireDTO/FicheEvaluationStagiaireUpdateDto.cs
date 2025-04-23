using StageManager.Models;
using System.ComponentModel.DataAnnotations;

public class FicheEvaluationStagiaireUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string NomPrenomStagiaire { get; set; }

    [Required]
    public string FormationStagiaire { get; set; }

    [Required]
    public string DureeStage { get; set; }

    [Required]
    public DateTime PeriodeDu { get; set; }

    [Required]
    public DateTime PeriodeAu { get; set; }

    [Required]
    public string StructureAccueil { get; set; }

    [Required]
    public int NombreSeancesPrevues { get; set; }

    [Required]
    public string NomPrenomEncadreur { get; set; }

    [Required]
    public string FonctionEncadreur { get; set; }

    [Required]
    public string ThemeStage { get; set; }

    [Required]
    public string MissionsConfieesAuStagiaire { get; set; }

    [Required]
    public NiveauEvaluationStagiaire RealisationMissionsConfiees { get; set; }

    [Required]
    public NiveauEvaluationStagiaire RespectDelaisProcedures { get; set; }

    [Required]
    public NiveauEvaluationStagiaire ComprehensionTravaux { get; set; }

    [Required]
    public NiveauEvaluationStagiaire AppreciationRenduTravail { get; set; }

    [Required]
    public NiveauEvaluationStagiaire UtilisationMoyensMisDisposition { get; set; }

    [Required]
    public NiveauEvaluationStagiaire NiveauConnaissances { get; set; }

    [Required]
    public NiveauEvaluationStagiaire CompetencesGenerales { get; set; }

    [Required]
    public NiveauEvaluationStagiaire AdaptationOrganisationMethodesTravail { get; set; }

    [Required]
    public NiveauEvaluationStagiaire PonctualiteAssiduite { get; set; }

    [Required]
    public NiveauEvaluationStagiaire RigueurSerieux { get; set; }

    [Required]
    public NiveauEvaluationStagiaire DisponibiliteMotivationEngagement { get; set; }

    [Required]
    public NiveauEvaluationStagiaire IntegrationSeinService { get; set; }

    [Required]
    public NiveauEvaluationStagiaire Aptitudes { get; set; }

    [Required]
    public NiveauEvaluationStagiaire TravailEquipe { get; set; }

    [Required]
    public NiveauEvaluationStagiaire CapaciteApprendreComprendre { get; set; }

    [Required]
    public NiveauEvaluationStagiaire ApplicationConnaissancesNouvelles { get; set; }

    [Required]
    public NiveauEvaluationStagiaire RechercheInformations { get; set; }

    [Required]
    public NiveauEvaluationStagiaire MethodeOrganisationTravail { get; set; }

    [Required]
    public NiveauEvaluationStagiaire AnalyseExplicationSynthese { get; set; }

    [Required]
    public NiveauEvaluationStagiaire Communication { get; set; }

    [Required]
    public string AppreciationGlobaleTuteur { get; set; }

    [MaxLength(500)]
    public string Observations { get; set; }

    [Required]
    public string NomPrenomEvaluateur { get; set; }
}
