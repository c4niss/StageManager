using System.ComponentModel.DataAnnotations;

namespace StageManager.Models
{
    public enum StagiaireStatus
    {
        EnCour,  // Statut par défaut
        Accepte,
        Refuse
    }
    public class Stagiaire : Utilisateur
    {
        [Required]
        [StringLength(100)]
        public string Universite { get; set; }
        [Required]
        [StringLength(100)]
        public string Specialite { get; set; }

        public StagiaireStatus Status { get; set; } = StagiaireStatus.EnCour;
        public DemandeDeStage DemandeDeStage { get; internal set; }
        public int? DemandeDeStageId { get; set; }
        public int? StageId { get; set; }
        public Stage Stage { get; set; }
        public int FicheEvaluationStagiaireId { get; set; }
        public FicheEvaluationStagiaire FicheEvaluationStagiaire { get; set; }
        public int FicheDePointageId { get; set; }
        public FicheDePointage FicheDePointage { get; set; }
        public int AttestationId { get; set; }
        public Attestation Attestation { get; set; }
    }
}
