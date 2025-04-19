using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DepartementDTO
{
    public class UpdateDepartementDto
    {
        [StringLength(20)]
        public string Nom { get; set; }

        public int? ChefDepartementId { get; set; }
    }
}