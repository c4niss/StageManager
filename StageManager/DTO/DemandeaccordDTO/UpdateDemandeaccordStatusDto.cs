using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdateDemandeaccordStatusDto
    {
        [Required(ErrorMessage = "Le statut est requis")]
        [EnumDataType(typeof(StatusAccord))]
        public StatusAccord Status { get; set; }

    }
}