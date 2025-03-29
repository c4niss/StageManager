using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ChefDepartementDTO
{
    public class ChefDepartementDto
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int DepartementId { get; set; }
        public string DepartementNom { get; set; }
    }
}
