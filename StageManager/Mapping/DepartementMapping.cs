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
                ChefDepartementNom = departement.ChefDepartement?.Nom,
                NombreEncadreurs = departement.Encadreurs?.Count ?? 0,
                NombreStagiairesActuels = departement.Encadreurs?
                    .Sum(e => e.NbrStagiaires) ?? 0
            };
        }

        public static Departement ToEntity(this CreateDepartementDto dto)
        {
            return new Departement
            {
                Nom = dto.Nom,
                ChefDepartementId = dto.ChefDepartementId
            };
        }
    }
}