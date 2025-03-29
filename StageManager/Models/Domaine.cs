using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Domaine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [ForeignKey("Departement")]
        public int DepartementId { get; set; }

        public Departement Departement { get; set; }
        public List<Encadreur> Encadreurs { get; set; }
        public List<Stage> Stages { get; set; }
    }
}