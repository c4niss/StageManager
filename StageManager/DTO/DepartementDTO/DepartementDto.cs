namespace StageManager.DTO.DepartementDTO
{
    using System.ComponentModel.DataAnnotations;

    namespace StageManager.DTOs
    {
        public class DepartementDto
        {
            public int Id { get; set; }

            [Required]
            [StringLength(20)]
            public string Nom { get; set; }

            public int? ChefDepartementId { get; set; }
            public string ChefDepartementNom { get; set; } // Nom complet du chef

            // Statistiques utiles (optionnel)
            public int NombreEncadreurs { get; set; }
            public int NombreStagiairesActuels { get; set; }
        }
    }
}