using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DomaineDTO
{
    public class UpdateDomaineDto
    {
        [StringLength(100)]
        public string Nom { get; set; }

        public int? DepartementId { get; set; }
    }
}