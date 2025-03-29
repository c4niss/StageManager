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

        // Changement : Rend la clé étrangère nullable
        [ForeignKey("Departement")]
        public int? DepartementId { get; set; }  // Devenu nullable

        // Suppression de la navigation inverse si non essentielle
        public Departement Departement { get; set; }

        // Liste modifiée pour éviter la référence circulaire
        [JsonIgnore]  // Important pour la sérialisation
        public List<Stage> Stages { get; set; }

        // Autres propriétés inchangées
        public List<Demandeaccord> Demandeaccords { get; set; }
        public List<FicheEvaluationStagiaire> FichesEvaluationStagiaire { get; set; }
        public List<FicheEvaluationEncadreur> FichesEvaluationEncadreur { get; set; }
    }
}