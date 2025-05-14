using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdateDemandeaccordStatusDto
    {
        [Required(ErrorMessage = "Le statut est requis")]
        [EnumDataType(typeof(StatusAccord))]
        public StatusAccord Status { get; set; }
        [StringLength(255, ErrorMessage = "Le commentaire ne peut pas dépasser 255 caractères")]
        public string? Commentaire { get; set; }

    }
}