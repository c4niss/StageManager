using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class CreateDemandeaccordDto
    {
        [Required]
        public IFormFile FichePieceJointe { get; set; } // Pour upload de fichier

        [Required]
        public List<int> StagiaireId { get; set; }

        [Required]
        public int ThemeId { get; set; }

        public int? EncadreurId { get; set; }
    }
}
