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
        public DateTime DateDemande { get; set; } = DateTime.UtcNow;

        [Required]
        public StatutDepotMemoire Statut { get; set; } = StatutDepotMemoire.EnAttente;

        [Required]
        [StringLength(255, ErrorMessage = "Le chemin du fichier ne peut dépasser 255 caractères")]
        public string CheminFichier { get; set; }

        [StringLength(500, ErrorMessage = "Le commentaire ne peut dépasser 500 caractères")]
        public string Commentaire { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        [ForeignKey("MembreDirection")]
        public int? MembreDirectionId { get; set; }

        // Navigation properties
        public Stage Stage { get; set; }
        public MembreDirection MembreDirection { get; set; }
        public Memoire Memoire { get; set; }
    }
}