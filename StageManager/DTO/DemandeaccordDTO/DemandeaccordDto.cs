using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class DemandeaccordDto
    {
        public int Id { get; set; }

        [Required]
        public string FichePieceJointe { get; set; }

        [EnumDataType(typeof(StatusAccord))]
        public StatusAccord Status { get; set; }

        public List<int> StagiaireId { get; set; }
        public string StagiaireNomComplet { get; set; }

        public int ThemeId { get; set; }
        public string ThemeNom { get; set; }

        public int? EncadreurId { get; set; }
        public string EncadreurNomComplet { get; set; }
    }
}
