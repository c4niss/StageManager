using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{

    public class DemandeDeStage
    {
        public enum StatusDemandeDeStage
        {
            EnCour,  // Statut par défaut
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
        public StatusDemandeDeStage Statut { get; set; } = StatusDemandeDeStage.EnCour;
        public List<Stagiaire> Stagiaires { get; set; }
        public int? DemandeaccordId { get; set; }
        public Demandeaccord Demandeaccord { get; set; }
        [ForeignKey("MembreDirection")]
        public int? MembreDirectionId { get; set; }
        public MembreDirection MembreDirection { get; set; }

    }
}