using StageManager.DTO.EncadreurDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class EncadreurMapping
    {
        public static EncadreurDto ToDto(this Encadreur encadreur)
        {
            return new EncadreurDto
            {
                Id = encadreur.Id,
                Nom = encadreur.Nom,
                Prenom = encadreur.Prenom,
                Email = encadreur.Email,
                Telephone = encadreur.Telephone,
                Fonction = encadreur.Fonction,
                EstDisponible = encadreur.EstDisponible,
                NbrStagiaires = encadreur.NbrStagiaires,
                StagiaireMax = encadreur.StagiaireMax,
                DepartementId = encadreur.DepartementId,
                DepartementNom = encadreur.Departement?.Nom
            };
        }

        public static Encadreur ToEntity(this CreateEncadreurDto dto)
        {
            return new Encadreur
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Telephone = dto.Telephone,
                MotDePasse = dto.MotDePasse, // Doit être hashé dans le contrôleur
                Role = "Encadreur",
                Fonction = string.Empty,
                EstDisponible = true,
                NbrStagiaires = 0,
                StagiaireMax = 5, // Valeur par défaut
                DepartementId = dto.DepartementId,
                EstActif = true
            };
        }
    }
}