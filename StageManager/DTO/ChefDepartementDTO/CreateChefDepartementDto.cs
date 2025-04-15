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
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [Phone]
        [StringLength(20)]
        public string Telephone { get; set; }
        [StringLength(255)]
        public string PhotoUrl { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string MotDePasse { get; set; }
        [Required]
        [StringLength(100)]
        public bool EstActif { get; set; }
    }
}
