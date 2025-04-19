using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StagiaireDTO
{
    public class UpdateStagiaireStatusDto
    {
        [Required]
        public StagiaireStatus Status { get; set; }
    }
}