using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ChefDepartementDTO
{
    public class UpdateChefDepartementDto
    {
        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(50)]
        public string Prenom { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        [Phone]
        public string Telephone { get; set; }
        public int? DepartementId { get; set; }
        public bool? EstActif { get; set; }
    }
}