using StageManager.DTO.ChefDepartementDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class ChefDepartementMapping
    {
        public static ChefDepartementDto ToDto(this ChefDepartement chef)
        {
            return new ChefDepartementDto
            {
                Id = chef.Id,
                Nom = chef.Nom,
                Prenom = chef.Prenom,
                Email = chef.Email,
                DepartementId = chef.DepartementId,
                DepartementNom = chef.Departement?.Nom
            };
        }
    }
}