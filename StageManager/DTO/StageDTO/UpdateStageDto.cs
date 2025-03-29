using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StageDTO
{
    public class UpdateStageDto
    {
        [StringLength(50)]
        public string? StagiaireGroup { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateDebut { get; set; }

        [DataType(DataType.Date)]
        [DateAfter("DateDebut")]
        public DateTime? DateFin { get; set; }

        [EnumDataType(typeof(StatutStage))]
        public StatutStage? Statut { get; set; }

        public int? EncadreurId { get; set; } // Pour réaffecter l'encadreur
    }
}
