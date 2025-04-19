using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DepartementDTO
{
    public class CreateDepartementDto
    {
        [Required]
        [StringLength(20)]
        public string Nom { get; set; }
    }
}