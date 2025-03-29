using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class MembreDirection : Utilisateur
    {
        [Required]
        [StringLength(100)]
        public string Fonction { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DatePrisePoste { get; set; } = DateTime.UtcNow;

        public List<Convention> Conventions { get; set; }
        public List<DemandeDeStage> DemandesDeStage { get; set; }
        public List<Attestation> Attestations { get; set; }
    }
}