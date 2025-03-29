using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ChefDepartementDTO
{
    public class AssignChefDepartementDto
    {
        [Required]
        public int DepartementId { get; set; }

        [Required]
        public int EncadreurId { get; set; } // L'encadreur à promouvoir
    }
}
