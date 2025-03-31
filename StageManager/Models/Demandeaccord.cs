using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public enum StatusAccord
    {
        EnAttente,
        Accepte,
        Refuse
    }

    public class Demandeaccord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FichePieceJointe { get; set; }

        [Required]
        public StatusAccord Status { get; set; } = StatusAccord.EnAttente;
        public List <Stagiaire> stagiaires { get; set; }
        [Required]
        public int ThemeId { get; set; }
        [Required]
        [ForeignKey("DemandeDeStage")]
        public int DemandeStageId { get; set; }

        [ForeignKey("Encadreur")]
        public int? EncadreurId { get; set; }

        public Theme Theme { get; set; }
        public DemandeDeStage DemandeDeStage { get; set; }
        public Encadreur Encadreur { get; set; }
    }
}