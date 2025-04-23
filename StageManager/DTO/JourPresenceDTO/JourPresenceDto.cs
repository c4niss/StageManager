using System.ComponentModel.DataAnnotations;

namespace StageManager.DTOs
{
    public class JourPresenceDto
    {
        [Required(ErrorMessage = "Le jour est obligatoire")]
        [Range(1, 31, ErrorMessage = "Le jour doit être compris entre 1 et 31")]
        public int Jour { get; set; }

        [Required(ErrorMessage = "Le jour de la semaine est obligatoire")]
        [StringLength(1, ErrorMessage = "Le jour de la semaine doit être représenté par une seule lettre (D, L, M, M, J, V, S)")]
        public string JourSemaine { get; set; }

        [Required(ErrorMessage = "Le statut de présence est obligatoire")]
        public bool EstPresent { get; set; }

        [StringLength(100, ErrorMessage = "Le commentaire ne peut pas dépasser 100 caractères")]
        public string Commentaire { get; set; }
    }
}
