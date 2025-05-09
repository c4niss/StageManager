using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class DemandeDeStage
    {
        public enum StatusDemandeDeStage
        {
            Enattente,  // Statut par défaut
            Accepte,
            Refuse
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateDemande { get; set; } = DateTime.Now;

        [Required]
        public string cheminfichier { get; set; }

        [Required]
        public StatusDemandeDeStage Statut { get; set; } = StatusDemandeDeStage.Enattente;

        public List<Stagiaire> Stagiaires { get; set; }

        public int? DemandeaccordId { get; set; }
        [ForeignKey(nameof(DemandeaccordId))]
        public Demandeaccord Demandeaccord { get; set; }

        public int? MembreDirectionId { get; set; }
        [ForeignKey(nameof(MembreDirectionId))]
        public MembreDirection MembreDirection { get; set; }
    }
}