using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Theme
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [ForeignKey("Demandeaccord")]
        public int DemandeaccordId { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        public Demandeaccord Demandeaccord { get; set; }
        public Stage Stage { get; set; }
    }
}