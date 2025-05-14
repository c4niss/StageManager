using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StageManager.Models
{
    public enum StatusAccord
    {
        EnCours,
        Accepte,
        Refuse,
        EnAttente
    }
    public enum NatureStage
    {
        StageImpregnation,
        StageFinEtude
    }
    public class Demandeaccord
    {
        public Demandeaccord()
        {
            // Initialisation des collections
            stagiaires = new List<Stagiaire>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public StatusAccord Status { get; set; } = StatusAccord.EnCours;

        public List<Stagiaire> stagiaires { get; set; }
        public int? StagiaireGroupeId { get; set; }
        [ForeignKey("ChefDepartement")]
        public int? ChefDepartementId { get; set; }

        [ForeignKey("Encadreur")]
        public int? EncadreurId { get; set; }

        public int? ThemeId { get; set; }

        [Required]
        [ForeignKey("DemandeDeStage")]
        public int DemandeStageId { get; set; }

        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        [Range(1, 3)]
        public int? NombreSeancesParSemaine { get; set; }

        [Range(1, 4)]
        public int? DureeSeances { get; set; }

        [StringLength(255)]
        public string? ServiceAccueil { get; set; }

        [StringLength(100)]
        public string? Nom { get; set; }

        [StringLength(100)]
        public string? Prenom { get; set; }

        [StringLength(255)]
        public string? UniversiteInstitutEcole { get; set; }

        [StringLength(255)]
        public string? FiliereSpecialite { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Telephone { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? DiplomeObtention { get; set; }

        public NatureStage? NatureStage { get; set; }
        public string? commentaire { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;
        public Convention Convention { get; set; }
        public int conventionId { get; set; }

        public DateTime? DateSoumissionStagiaire { get; set; }

        public bool RappelJour6Envoye { get; set; } = false;
        public bool RappelJour7Envoye { get; set; } = false;

        // Relations avec annotation nullable pour correspondre aux clés étrangères
        public virtual Theme? Theme { get; set; }
        public virtual DemandeDeStage DemandeDeStage { get; set; } = null!;
        public virtual Encadreur? Encadreur { get; set; }
        public virtual ChefDepartement? ChefDepartement { get; set; }
    }
}