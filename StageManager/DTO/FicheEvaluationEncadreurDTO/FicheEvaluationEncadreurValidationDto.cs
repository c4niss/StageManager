using System.ComponentModel.DataAnnotations;

namespace StageManager.DTOs
{
    public class FicheEvaluationEncadreurValidationDto
    {
        [Required(ErrorMessage = "Le statut de validation est obligatoire")]
        public bool EstValide { get; set; }
    }
}