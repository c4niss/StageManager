using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdateDemandeaccordStatusDto
    {
        [Required]
        [EnumDataType(typeof(StatusAccord))]
        public StatusAccord Status { get; set; }

        public string Commentaire { get; set; }
    }
}
