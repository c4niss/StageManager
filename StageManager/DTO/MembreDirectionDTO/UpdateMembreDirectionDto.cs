using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.MembreDirectionDTO
{
    public class UpdateMembreDirectionDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nom { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Prenom { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Telephone { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(100)]
        public string Fonction { get; set; }
        [StringLength(100, MinimumLength = 8)]
        public string MotDePasse { get; set; }
    }
}