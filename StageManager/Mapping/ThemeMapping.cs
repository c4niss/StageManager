using StageManager.DTO.ThemeDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class ThemeMapping
    {
        public static ThemeDto ToDto(this Theme theme)
        {
            return new ThemeDto
            {
                Id = theme.Id,
                Nom = theme.Nom,
                DemandeaccordId = theme.DemandeaccordId,
                StageId = theme.StageId,
                StageNom = theme.Stage?.StagiaireGroup ?? string.Empty
            };
        }
    }
}
