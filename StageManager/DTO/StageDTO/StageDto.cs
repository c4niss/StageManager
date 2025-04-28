using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StageDTO
{
    public class StageDto
    {
        public int Id { get; set; }

        [Required]
        public string StagiaireGroup { get; set; } // Ex: "Groupe A 2023"

        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        [EnumDataType(typeof(StatutStage))]
        public StatutStage Statut { get; set; }

        // Références avec infos minimales
        public int? ConventionId { get; set; }
        public int? DepartementId { get; set; }
        public string DepartementNom { get; set; }

        public int? EncadreurId { get; set; }
        public string EncadreurNomComplet { get; set; }

        // Optionnel : Liste simplifiée des stagiaires
        public List<StagiaireInfoDto> Stagiaires { get; set; }
    }

    public class StagiaireInfoDto
    {
        public int Id { get; set; }
        public string NomComplet { get; set; }
        public string Email { get; set; }
    }
}