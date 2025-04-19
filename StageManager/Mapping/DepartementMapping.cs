using StageManager.DTO.DepartementDTO;
using StageManager.DTO.DepartementDTO.StageManager.DTOs;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class DepartementMapping
    {
        public static DepartementDto ToDto(this Departement departement)
        {
            return new DepartementDto
            {
                Id = departement.Id,
                Nom = departement.Nom,
                ChefDepartementId = departement.ChefDepartementId,
                ChefDepartementNom = departement.ChefDepartement != null
                    ? $"{departement.ChefDepartement.Nom} {departement.ChefDepartement.Prenom}"
                    : string.Empty,
                NombreEncadreurs = departement.Encadreurs?.Count ?? 0,
            };
        }
    }
}