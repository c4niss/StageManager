using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class FicheEvaluationEncadreur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateEvaluation { get; set; }

        [Required]
        [ForeignKey("Encadreur")]
        public int EncadreurId { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        public Encadreur Encadreur { get; set; }
        public Stage Stage { get; set; }
    }
}