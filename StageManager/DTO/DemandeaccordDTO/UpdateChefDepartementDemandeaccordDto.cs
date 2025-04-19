using System;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdateChefDepartementDemandeaccordDto
    {
        [Required(ErrorMessage = "Le service d'accueil est requis")]
        [StringLength(255)]
        public string? ServiceAccueil { get; set; }

        [Required(ErrorMessage = "La date de début est requise")]
        public DateTime? DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise")]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "Le nombre de séances par semaine est requis")]
        [Range(1, 3, ErrorMessage = "Le nombre de séances doit être entre 1 et 3")]
        public int? NombreSeancesParSemaine { get; set; }

        [Required(ErrorMessage = "La durée des séances est requise")]
        [Range(1, 4, ErrorMessage = "La durée des séances doit être entre 1 et 4 heures")]
        public int? DureeSeances { get; set; }
    }
}