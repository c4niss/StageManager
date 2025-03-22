using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeDeStageDTO
{
    public class DemandeDeStageCreateDto
    {
        [Required]
        public string CheminFichier { get; set; }

        [Required]
        public List<int> StagiaireIds { get; set; }
    }
}
