using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ConventionDTO
{
    public class UpdateConventionDto
    {
        [Required]
        [StringLength(255)]
        public string CheminFichier { get; set; }
        public Convention.Statusconvention Status { get; set; }

    }
}