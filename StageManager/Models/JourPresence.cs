using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StageManager.Models
{
    public class JourPresence
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 31)]
        public int Jour { get; set; } // Numéro du jour dans le mois (1-31)

        [Required]
        public DayOfWeek JourSemaine { get; set; } // Jour de la semaine (D, L, M, M, J, V, S)

        [Required]
        public bool EstPresent { get; set; } // True si présent, False si absent

        // Champ optionnel pour les commentaires (retard, absence justifiée, etc.)
        [StringLength(100)]
        public string Commentaire { get; set; }

        // Relation avec le mois de pointage
        [Required]
        [ForeignKey("PointageMois")]
        public int PointageMoisId { get; set; }

        public virtual PointageMois PointageMois { get; set; }
    }
}
