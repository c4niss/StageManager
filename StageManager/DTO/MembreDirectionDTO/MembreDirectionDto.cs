using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.MembreDirectionDTO
{
    public class MembreDirectionDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
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
