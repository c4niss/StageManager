using System.ComponentModel.DataAnnotations;

namespace StageManager.Models
{
    public abstract class Utilisateur
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Phone]
        [StringLength(20)]
        public string Telephone { get; set; }
        [Required]
        [StringLength(255)]
        public string MotDePasse { get; set; }
        [Required]
        [StringLength(30)]
        public string Role { get; set; }
        [Required]
        public DateTime DateCreation { get; set; } = System.DateTime.Now;
        [Required]
        public bool EstActif { get; set; }
        [StringLength(255)]
        public string PhotoUrl { get; set; }
    }
}
