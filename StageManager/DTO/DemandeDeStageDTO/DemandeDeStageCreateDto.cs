using StageManager.Models;
using System.ComponentModel.DataAnnotations;
using static StageManager.Models.DemandeDeStage;
using static StageManager.Models.Stagiaire;

namespace StageManager.DTO.DemandeDeStageDTO
{
    public class DemandeDeStageCreateDto
    {
        [Required]
        [StringLength(500, ErrorMessage = "Le chemin du fichier est trop long")]
        public string CheminFichier { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Au moins un stagiaire requis")]
        public List<int> StagiaireIds { get; set; }
        public DateTime DateDemande { get; set; } = DateTime.Now;

        [Range(0, 2)]
        public StatusDemandeDeStage Statut { get; set; } = StatusDemandeDeStage.EnCour;
    }

}