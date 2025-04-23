using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StageManager.DTOs
{
    public class PointageMoisDto
    {
        [Required(ErrorMessage = "Le mois est obligatoire")]
        [StringLength(20, ErrorMessage = "Le nom du mois ne peut pas dépasser 20 caractères")]
        public string Mois { get; set; }

        [Required(ErrorMessage = "L'année est obligatoire")]
        [Range(2000, 2100, ErrorMessage = "L'année doit être comprise entre 2000 et 2100")]
        public int Annee { get; set; }

        public List<JourPresenceDto> JoursPresence { get; set; } = new List<JourPresenceDto>();
    }
}