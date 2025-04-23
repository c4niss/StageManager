using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public enum StatutDepotMemoire
    {
        EnAttente,
        Valide,
        Rejete,
        Archive
    }

    public class DemandeDepotMemoire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateDemande { get; set; } = DateTime.Now;
        [Required]
        public StatutDepotMemoire Statut { get; set; } = StatutDepotMemoire.EnAttente;

        // Informations sur le thème du mémoire
        [Required]
        [StringLength(200, ErrorMessage = "Le thème ne peut dépasser 200 caractères")]
        public Theme Theme { get; set; }
        public int themeId { get; set; }
        // Informations sur l'étudiant
        [Required]
        [StringLength(100)]
        public string NomPrenomEtudiants { get; set; }

        // Informations sur l'encadreur
        [Required]
        [StringLength(100)]
        public string NomPrenomEncadreur { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateValidation { get; set; }
        // Relations
        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        [ForeignKey("MembreDirection")]
        public int? MembreDirectionId { get; set; }

        [ForeignKey("Encadreur")]
        public int EncadreurId { get; set; }
        public virtual Stage Stage { get; set; }
        public virtual MembreDirection MembreDirection { get; set; }
        public virtual Encadreur Encadreur { get; set; }
        public virtual Memoire Memoire { get; set; }
    }
}
