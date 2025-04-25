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
                Username = encadreur.Username,
                Telephone = encadreur.Telephone,
                Fonction = encadreur.Fonction,
                EstDisponible = encadreur.EstDisponible,
                NbrStagiaires = encadreur.NbrStagiaires,
                StagiaireMax = encadreur.StagiaireMax,
                DepartementId = encadreur.DepartementId,
                DepartementNom = encadreur.Departement?.Nom,
                DomaineId = encadreur.DomaineId,
                DomaineNom = encadreur.Domaine?.Nom
            };
        }
    }
}