using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StageDTO
{
    public class CreateStageDto
    {
        [Required]
        [StringLength(50)]
        public string StagiaireGroup { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateAfter("DateDebut", ErrorMessage = "Doit être après DateDebut")]
        public DateTime DateFin { get; set; }

        [Required]
        public int ConventionId { get; set; }

        [Required]
        public int DepartementId { get; set; }

        [Required]
        public int EncadreurId { get; set; }

        public List<int> StagiaireIds { get; set; } // IDs des stagiaires à associer
    }

    // Validateur personnalisé (à ajouter dans votre projet)
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public DateAfterAttribute(string comparisonProperty) => _comparisonProperty = comparisonProperty;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var currentValue = (DateTime)value;
            var property = context.ObjectType.GetProperty(_comparisonProperty);
            var comparisonValue = (DateTime)property.GetValue(context.ObjectInstance);

            return currentValue > comparisonValue
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
