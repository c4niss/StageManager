using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StagiaireDTO
{
    public class StagiaireUpdateDto
    {
        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(50)]
        public string Prenom { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [Phone]
        [StringLength(20)]
        public string Telephone { get; set; }

        [StringLength(100)]
        public string Universite { get; set; }

        [StringLength(100)]
        public string Specialite { get; set; }

        [StringLength(255)]
        public string PhotoUrl { get; set; }
        public StagiaireStatus Status { get; set; }

    }
}