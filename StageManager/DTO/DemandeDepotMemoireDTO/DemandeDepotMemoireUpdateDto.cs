using System.ComponentModel.DataAnnotations;

namespace StageManager.DTOs
{
    public class DemandeDepotMemoireUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public int stageId { get; set; }
        public int? MembreDirectionId { get; set; }
    }
}
