using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DomaineDTO
{
    public class CreateDomaineDto
    {
        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        public int DepartementId { get; set; }
    }
}