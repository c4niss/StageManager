using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeDeStageDTO
{
    public class DemandeDeStageUpdateStatusDto
    {
        [Required]
        public DemandeDeStage.StatusDemandeDeStage Statut { get; set; }
    }
}
