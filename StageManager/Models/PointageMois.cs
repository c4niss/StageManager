using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StageManager.Models
{
    public class PointageMois
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Mois { get; set; } // Nom du mois (ex: Janvier, Février, etc.)

        [Required]
        [Range(2000, 2100)]
        public int Annee { get; set; } // Année du mois de pointage

        // Relation avec la fiche de pointage
        [Required]
        [ForeignKey("FichePointage")]
        public int FichePointageId { get; set; }

        public virtual FicheDePointage FicheDePointage { get; set; }

        // Collection des jours de présence pour ce mois
        public virtual ICollection<JourPresence> JoursPresence { get; set; }
    }
}
