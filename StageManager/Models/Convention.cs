using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Convention
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateDepot { get; set; } = DateTime.UtcNow;

        [Required]
        public bool EstValidee { get; set; } = false;

        [Required]
        [StringLength(255)]
        public string CheminFichier { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        [Required]
        [ForeignKey("MembreDirection")]
        public int MembreDirectionId { get; set; }

        public Stage Stage { get; set; }
        public MembreDirection MembreDirection { get; set; }
    }
}