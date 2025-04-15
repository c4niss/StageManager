using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class CreateDemandeaccordDto
    {
        [Required]
        public List<int> StagiaireId { get; set; }

        [Required]
        public int DemandeStageId { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}