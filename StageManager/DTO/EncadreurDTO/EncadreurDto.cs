namespace StageManager.DTO.EncadreurDTO
{
    using StageManager.Models;
    using System.ComponentModel.DataAnnotations;

    public class EncadreurDto
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
        public string Telephone { get; set; }
        [StringLength(100)]
        public string Fonction { get; set; }

        public bool EstDisponible { get; set; }
        public int NbrStagiaires { get; set; }
        public int StagiaireMax { get; set; }

        // Références aux autres entités
        public int? DepartementId { get; set; }
        public string DepartementNom { get; set; }

        public int? DomaineId { get; set; }
        public string DomaineNom { get; set; }
        public bool EstActif { get; set; }
    }
}