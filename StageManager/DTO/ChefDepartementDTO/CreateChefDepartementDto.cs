using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ChefDepartementDTO
{
    public class CreateChefDepartementDto
    {
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        [Phone]
        public string Telephone { get; set; }

        [Required]
        [StringLength(255)]
        public string MotDePasse { get; set; }

        [Required]
        public int DepartementId { get; set; }

        public bool EstActif { get; set; } = true;
    }
}
