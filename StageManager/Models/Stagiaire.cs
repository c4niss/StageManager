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
        public object? DemandeDeStageId { get; internal set; }
    }
}
