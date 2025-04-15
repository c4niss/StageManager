using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.MembreDirectionDTO
{
    public class MinimalMembreDirectionDto
    {
        public int Id { get; set; }

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

    }
}
