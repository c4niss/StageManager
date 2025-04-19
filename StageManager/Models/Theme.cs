using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        // Make StageId nullable
        [ForeignKey("Stage")]
        public int? StageId { get; set; }

        [JsonIgnore]
        public Demandeaccord Demandeaccord { get; set; }

        [JsonIgnore]
        public Stage? Stage { get; set; }
    }
}