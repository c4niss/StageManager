using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.EncadreurDTO
{
    public class CreateEncadreurDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string MotDePasse { get; set; }

        [Phone]
        public string Telephone { get; set; }

        [Required]
        public int DepartementId { get; set; }
    }
}
