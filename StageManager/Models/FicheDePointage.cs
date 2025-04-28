using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class FicheDePointage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Required]
        public string NomPrenomStagiaire { get; set; }

        [Required]
        public string StructureAccueil { get; set; }

        [Required]
        public string NomQualitePersonneChargeSuivi { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDebutStage { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFinStage { get; set; }

        public NatureStage NatureStage { get; set; }

        // Données de pointage (stockées sous forme de JSON ou autre format approprié)
        public string DonneesPointage { get; set; }

        public bool EstValide { get; set; }

        // Relations
        [Required]
        [ForeignKey("Stagiaire")]
        public int StagiaireId { get; set; }

        [Required]
        [ForeignKey("Encadreur")]
        public int EncadreurId { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        public virtual Stagiaire Stagiaire { get; set; }
        public virtual Encadreur Encadreur { get; set; }
        public virtual Stage Stage { get; set; }

        // Collection des mois de pointage pour cette fiche
        public virtual ICollection<PointageMois> PointageMois { get; set; } = new List<PointageMois>();
    }
}
