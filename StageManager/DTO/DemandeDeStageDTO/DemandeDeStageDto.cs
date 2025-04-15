using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeDeStageDTO
{
    public class DemandeDeStageDto
    {
        public int Id { get; set; }
        public DateTime DateDemande { get; set; }
        public string CheminFichier { get; set; }
        public DemandeDeStage.StatusDemandeDeStage Statut { get; set; }
        public List<StagiaireDTO.StagiaireDto> Stagiaires { get; set; }
        public MembreDirectionDTO.MembreDirectionDto MembreDirection { get; set; }
    }
}