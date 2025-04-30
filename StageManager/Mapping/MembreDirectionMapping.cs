using StageManager.DTO.MembreDirectionDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class MembreDirectionMapping
    {
        public static MembreDirectionDto ToDto(this MembreDirection membre)
        {
            return new MembreDirectionDto
            {
                Id = membre.Id,
                Nom = membre.Nom,
                Prenom = membre.Prenom,
                Username = membre.Username,
                Email = membre.Email,
                Telephone = membre.Telephone,
                Fonction = membre.Fonction,
                DatePrisePoste = membre.DatePrisePoste,
                EstActif = membre.EstActif,
            };
        }

        public static MembreDirection ToEntity(this CreateMembreDirectionDto dto)
        {
            return new MembreDirection
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Username = dto.Username,
                Telephone = null, // ou une valeur par défaut si nécessaire
                Fonction = dto.Fonction,
                MotDePasse = dto.MotDePasse, // Doit être hashé dans le contrôleur
                Role = "MembreDirection",
                DatePrisePoste = DateTime.Now,
                EstActif = true
            };
        }
    }
}