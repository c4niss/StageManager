using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdateStagiaireDemandeaccordDto
    {
        [Required(ErrorMessage = "L'université/institut/école est requis")]
        [StringLength(255)]
        public string? UniversiteInstitutEcole { get; set; }

        [Required(ErrorMessage = "La filière/spécialité est requise")]
        [StringLength(255)]
        public string? FiliereSpecialite { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est requis")]
        [Phone]
        [StringLength(20)]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Le thème est requis")]
        public int? ThemeId { get; set; }

        [Required(ErrorMessage = "Le diplôme d'obtention est requis")]
        [StringLength(255)]
        public string? DiplomeObtention { get; set; }

        [Required(ErrorMessage = "La nature du stage est requise")]
        [EnumDataType(typeof(NatureStage))]
        public NatureStage? NatureStage { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100)]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100)]
        public string? Prenom { get; set; }
    }
}