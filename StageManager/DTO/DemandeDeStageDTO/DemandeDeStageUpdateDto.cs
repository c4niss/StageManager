using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeDeStageDTO
{
    public class DemandeDeStageUpdateDto
    {
        public string CheminFichier { get; set; }
        public DemandeDeStage.StatusDemandeDeStage Statut { get; set; }
    }
}
