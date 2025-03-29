using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Avenant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateAvenant { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime NouvelleFinprevue { get; set; }

        [Required]
        [StringLength(500)]
        public string Motif { get; set; }

        [Required]
        public bool EstAccepter { get; set; } = false;

        public DateTime? DateDecision { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        [Required]
        [ForeignKey("Encadreur")]
        public int EncadreurId { get; set; }

        public Stage Stage { get; set; }
        public Encadreur Encadreur { get; set; }
    }
}