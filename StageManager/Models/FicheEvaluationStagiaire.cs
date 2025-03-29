using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class FicheEvaluationStagiaire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateEvaluation { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [ForeignKey("Stagiaire")]
        public int StagiaireId { get; set; }

        [Required]
        [ForeignKey("Encadreur")]
        public int EncadreurId { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        public Stagiaire Stagiaire { get; set; }
        public Encadreur Encadreur { get; set; }
        public Stage Stage { get; set; }
    }
}