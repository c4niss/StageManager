using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdatemembreDirectionDemandeaccorDto
    {
        [Required(ErrorMessage = "La nature du stage est requise")]
        [EnumDataType(typeof(NatureStage))]
        public NatureStage? NatureStage { get; set; }

        [Required(ErrorMessage = "Le thème est requis")]
        public string ThemeNom { get; set; }
        [Required(ErrorMessage = "Le numéro du département est requis")]
        public int DepartementId { get; set; }

        [Required(ErrorMessage = "Le domaine est requis")]
        public int DomaineId { get; set; }
        [Required(ErrorMessage = "L'université / institut / école est requise")]
        public string UniversiteInstitutEcole { get; set; }

        [Required(ErrorMessage = "La filière / spécialité est requise")]
        public string FiliereSpecialite { get; set; }

        [Required(ErrorMessage = "Le diplôme d'obtention est requis")]
        public string DiplomeObtention { get; set; }
    }
}