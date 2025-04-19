using StageManager.DTO.ConventionDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class ConventionMapping
    {
        public static ConventionDto ToDto(this Convention convention)
        {
            return new ConventionDto
            {
                Id = convention.Id,
                DateDepot = convention.DateDepot,
                Status = convention.status,
                CheminFichier = convention.CheminFichier,
                StageId = convention.StageId,
                MembreDirectionId = convention.MembreDirectionId,
                MembreDirectionNom = convention.MembreDirection?.Nom
            };
        }
    }
}