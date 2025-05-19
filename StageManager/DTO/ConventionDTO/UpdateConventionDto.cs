using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ConventionDTO
{
    public class UpdateConventionDto
    {
        
        [StringLength(255)]
        public string? CheminFichier { get; set; }
        public Convention.Statusconvention Status { get; set; }
        public string? Commentaire { get; set; }

    }
}