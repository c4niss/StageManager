using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdatemembreDirectionDemandeaccorDto
    {
        [Required(ErrorMessage = "La nature du stage est requise")]
        [EnumDataType(typeof(NatureStage))]
        public NatureStage? NatureStage { get; set; }

        [Required(ErrorMessage = "Le thème est requis")]
        public int? ThemeId { get; set; }
    }
}