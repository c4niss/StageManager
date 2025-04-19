using StageManager.DTO.DomaineDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class DomaineMapping
    {
        public static DomaineDto ToDto(this Domaine domaine)
        {
            return new DomaineDto
            {
                Id = domaine.Id,
                Nom = domaine.Nom,
                DepartementId = domaine.DepartementId,
                DepartementNom = domaine.Departement?.Nom ?? string.Empty,
                NombreEncadreurs = domaine.Encadreurs?.Count ?? 0,
                NombreStages = domaine.Stages?.Count ?? 0
            };
        }
    }
}