using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.EncadreurDTO
{
    public class UpdateEncadreurDto
    {

        [StringLength(50, MinimumLength = 2)]
        public string Nom { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Prenom { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Telephone { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(100)]
        public string Fonction { get; set; }


        public bool? EstDisponible { get; set; }
        public int? DepartementId { get; set; }
        public int? DomaineId { get; set; }
        public bool? EstActif { get; set; }
    }
}