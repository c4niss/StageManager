using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StageManager.Models
{
    public class Departement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        // Changement : Rend la clé étrangère nullable
        [ForeignKey("ChefDepartement")]
        public int? ChefDepartementId { get; set; }  // Devenu nullable

        // Navigation principale
        public ChefDepartement ChefDepartement { get; set; }

        // Listes avec JsonIgnore
        [JsonIgnore]
        public List<Encadreur> Encadreurs { get; set; }

        [JsonIgnore]
        public List<Domaine> Domaines { get; set; }
    }
}