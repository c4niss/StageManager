using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Convention
    {
        public enum Statusconvention
        {
            EnAttente,
            Accepte,
            Refuse
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateDepot { get; set; } = DateTime.Now;

        [Required]
        public Statusconvention status { get; set; } = Statusconvention.EnAttente;

        [StringLength(255)]
        public string? CheminFichier { get; set; }
        public string? Commentaire { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int? StageId { get; set; }
        [ForeignKey("DemandeAccord")]
        public int DemandeAccordId { get; set; }
        public Demandeaccord DemandeAccord { get; set; }
        [Required]
        [ForeignKey("MembreDirection")]
        public int? MembreDirectionId { get; set; }
        public Stage? Stage { get; set; }
        public MembreDirection MembreDirection { get; set; }
    }
}