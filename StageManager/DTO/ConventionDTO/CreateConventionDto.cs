using System.ComponentModel.DataAnnotations;
using static StageManager.Models.Convention;
using static StageManager.Models.DemandeDeStage;

namespace StageManager.DTO.ConventionDTO
{
    public class CreateConventionDto
    {
        public DateTime DateCreation { get; set; } = DateTime.Now;
        [Required]
        [StringLength(255)]
        public string CheminFichier { get; set; }
        [Required]
        public int MembreDirectionId { get; set; }

        public int? StageId { get; set; }
        [Required]
        public int DemandeAccordId { get; set; }
    }
}