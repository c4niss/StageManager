using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DepartementDTO
{
    public class UpdateDepartementDto
    {
        [StringLength(100, MinimumLength = 3)]
        public string? Nom { get; set; }

        public int? ChefDepartementId { get; set; } // Pour changer le chef
    }
}
