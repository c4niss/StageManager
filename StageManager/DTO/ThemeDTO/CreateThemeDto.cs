using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ThemeDTO
{
    public class CreateThemeDto
    {
        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        public int DemandeaccordId { get; set; }

        // Make StageId optional
        public int? StageId { get; set; }
    }
}