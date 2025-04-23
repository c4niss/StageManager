using System.ComponentModel.DataAnnotations;

namespace StageManager.DTOs
{
    public class FichePointageValidationDto
    {
        [Required(ErrorMessage = "L'identifiant de la fiche de pointage est obligatoire")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le statut de validation est obligatoire")]
        public bool EstValide { get; set; }
    }
}
