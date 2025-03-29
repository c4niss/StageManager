using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Attestation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateGeneration { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(255, ErrorMessage = "Le chemin du fichier ne peut dépasser 255 caractères")]
        public string CheminFichier { get; set; }

        [Required]

        public bool EstDelivree { get; set; } = false;

        [Required]
        [ForeignKey("Stagiaire")]
        public int StagiaireId { get; set; }

        [Required]
        [ForeignKey("MembreDirection")]
        public int MembreDirectionId { get; set; }

        // Navigation properties
        public Stagiaire Stagiaire { get; set; }
        public MembreDirection MembreDirection { get; set; }
    }
}