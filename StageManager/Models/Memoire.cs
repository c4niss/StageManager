using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StageManager.Models
{
    public class Memoire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titre { get; set; }

        [Required]
        [StringLength(255)]
        public string CheminFichier { get; set; }

        [Required]
        public DateTime DateDepot { get; set; } = DateTime.Now;

        // Modification des clés étrangères
        [ForeignKey("DemandeDepotMemoire")]
        public int DemandeDepotMemoireId { get; set; }

        [ForeignKey("Stage")]
        public int StageId { get; set; }

        // Navigation properties avec JsonIgnore
        [JsonIgnore]
        public DemandeDepotMemoire DemandeDepotMemoire { get; set; }

        [JsonIgnore]
        public Stage Stage { get; set; }
    }
}