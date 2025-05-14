using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class ChefDepartement : Utilisateur
    {
        [Required]
        [ForeignKey("Departement")]
        public int DepartementId { get; set; }
        [Required]
        [StringLength(100)]
        public string Fonction { get; set; }
        public Departement Departement { get; set; }
        public List<Demandeaccord> Demandeaccords { get; set; }
    }
}