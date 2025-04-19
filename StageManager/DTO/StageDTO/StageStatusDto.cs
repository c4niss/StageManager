using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StageDTO
{
    public class StageStatusDto
    {
        [Required]
        [EnumDataType(typeof(StatutStage))]
        public StatutStage Statut { get; set; }

        [StringLength(500)]
        public string? Raison { get; set; } // Pour "Annule" ou "Prolonge"
    }
}