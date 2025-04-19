using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StageManager.Models
{
    public class Encadreur : Utilisateur
    {
        [StringLength(100)]
        public string Fonction { get; set; }

        public DateTime? DateDemande { get; set; }

        [Required]
        public bool EstDisponible { get; set; } = true;

        [Required]
        public int NbrStagiaires { get; set; } = 0;

        [Required]
        public int StagiaireMax { get; set; } = 3;
        public int? AvenantId { get; set; }
        public Avenant Avenant { get; set; }
        [ForeignKey("Departement")]
        public int? DepartementId { get; set; }
        public Departement Departement { get; set; }
        [ForeignKey("Domaine")]
        public int? DomaineId { get; set; }
        public Domaine Domaine { get; set; }
        [JsonIgnore]
        public List<Stage> Stages { get; set; }
        public List<Demandeaccord> Demandeaccords { get; set; }
        public List<FicheEvaluationStagiaire> FichesEvaluationStagiaire { get; set; }
        public List<FicheEvaluationEncadreur> FichesEvaluationEncadreur { get; set; }
        public List<FicheDePointage> FichesDePointage { get; set; }
    }
}