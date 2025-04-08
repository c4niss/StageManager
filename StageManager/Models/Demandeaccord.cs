using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public enum StatusAccord
    {
        EnAttente,
        Accepte,
        Refuse
    }

    public class Demandeaccord
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public StatusAccord Status { get; set; } = StatusAccord.EnAttente;
        public List<Stagiaire> stagiaires { get; set; }
        [Required]
        public int ThemeId { get; set; }
        [Required]
        [ForeignKey("DemandeDeStage")]
        public int DemandeStageId { get; set; }

        [ForeignKey("Encadreur")]
        public int? EncadreurId { get; set; }

        // Ajout des propriétés pour les périodes de stage
        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public DateTime DateFin { get; set; }

        // Ajout des propriétés pour les informations sur les séances
        [Required]
        [Range(1, 7)]
        public int NombreSeancesParSemaine { get; set; }

        [Required]
        [Range(1, 8)]
        public int DureeSeances { get; set; }

        // Relations
        public Theme Theme { get; set; }
        public DemandeDeStage DemandeDeStage { get; set; }
        public Encadreur Encadreur { get; set; }
        public DateTime DateCreation { get; internal set; }
    }
}