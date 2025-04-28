using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public enum NiveauEvaluationStagiaire
    {
        Insatisfaisant = 1,
        PeuInsatisfaisant = 2,
        Satisfaisant = 3,
        TresSatisfaisant = 4
    }

    public class FicheEvaluationStagiaire
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateCreation { get; set; } = DateTime.Now;
        // Identification du stagiaire
        [Required]
        public string NomPrenomStagiaire { get; set; }

        [Required]
        public string FormationStagiaire { get; set; }

        // Informations sur le stage
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

        // Informations sur l'encadreur
        [Required]
        public string NomPrenomEncadreur { get; set; }

        [Required]
        public string FonctionEncadreur { get; set; }

        // Thème et missions
        [Required]
        public string ThemeStage { get; set; }

        [Required]
        public string MissionsConfieesAuStagiaire { get; set; }

        // Critères d'évaluation
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

        // Sections d'évaluation spécifiques visibles sur l'image
        [Required]
        public NiveauEvaluationStagiaire RechercheInformations { get; set; }

        [Required]
        public NiveauEvaluationStagiaire MethodeOrganisationTravail { get; set; }

        [Required]
        public NiveauEvaluationStagiaire AnalyseExplicationSynthese { get; set; }

        [Required]
        public NiveauEvaluationStagiaire Communication { get; set; }

        // Appréciation globale du tuteur
        [Required]
        public string AppreciationGlobaleTuteur { get; set; }

        // Champ d'observation
        [MaxLength(500)]
        public string Observations { get; set; }

        // Informations de signature
        [Required]
        public string NomPrenomEvaluateur { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateEvaluation { get; set; }

        // Relations
        [Required]
        [ForeignKey("Stagiaire")]
        public int StagiaireId { get; set; }

        [Required]
        [ForeignKey("Encadreur")]
        public int? EncadreurId { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        public virtual Stagiaire Stagiaire { get; set; }
        public virtual Encadreur Encadreur { get; set; }
        public virtual Stage Stage { get; set; }
    }
}
