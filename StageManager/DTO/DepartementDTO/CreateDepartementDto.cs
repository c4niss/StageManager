using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DepartementDTO
{
    public class CreateDepartementDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "3-100 caractères")]
        public string Nom { get; set; }

        public int? ChefDepartementId { get; set; } // Optionnel à la création
    }
}
