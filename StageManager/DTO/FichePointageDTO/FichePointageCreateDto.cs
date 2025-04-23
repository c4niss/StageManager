using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTOs
{
    public class FichePointageCreateDto
    {
        [Required(ErrorMessage = "Le nom et prénom du stagiaire sont obligatoires")]
        [StringLength(100, ErrorMessage = "Le nom et prénom ne peuvent pas dépasser 100 caractères")]
        public string NomPrenomStagiaire { get; set; }

        [Required(ErrorMessage = "La structure d'accueil est obligatoire")]
        [StringLength(100, ErrorMessage = "La structure d'accueil ne peut pas dépasser 100 caractères")]
        public string StructureAccueil { get; set; }

        [Required(ErrorMessage = "Le nom et la qualité de la personne chargée du suivi sont obligatoires")]
        [StringLength(100, ErrorMessage = "Le nom et la qualité ne peuvent pas dépasser 100 caractères")]
        public string NomQualitePersonneChargeSuivi { get; set; }

        [Required(ErrorMessage = "La date de début de stage est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateDebutStage { get; set; }

        [Required(ErrorMessage = "La date de fin de stage est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateFinStage { get; set; }

        [Required(ErrorMessage = "La nature du stage est obligatoire")]
        public NatureStage NatureStage { get; set; }

        [Required(ErrorMessage = "L'identifiant du stagiaire est obligatoire")]
        public int StagiaireId { get; set; }

        [Required(ErrorMessage = "L'identifiant de l'encadreur est obligatoire")]
        public int EncadreurId { get; set; }

        [Required(ErrorMessage = "L'identifiant du stage est obligatoire")]
        public int StageId { get; set; }
    }
}
