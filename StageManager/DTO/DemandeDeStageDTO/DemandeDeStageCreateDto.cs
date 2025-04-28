using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeDeStageDTO
{
    public class DemandeDeStageCreateDto
    {
        [Required]
        public string CheminFichier { get; set; }
        [Required]
        public List<int> StagiaireIds { get; set; }
        public DateTime DateDemande { get; set; } = DateTime.Now;
        // Add MembreDirectionId to the DTO
        public int? MembreDirectionId { get; set; }
        public DemandeDeStage.StatusDemandeDeStage Statut { get; set; } = DemandeDeStage.StatusDemandeDeStage.EnCours;
    }
}
