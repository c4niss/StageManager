using StageManager.DTO.StagiaireDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class StagiaireMapping
    {
        public static StagiaireDto ToDto(this Stagiaire stagiaire)
        {
            return new StagiaireDto
            {
                Nom = stagiaire.Nom,
                Prenom = stagiaire.Prenom,
                Email = stagiaire.Email,
                Telephone = stagiaire.Telephone,
                Universite = stagiaire.Universite,
                Specialite = stagiaire.Specialite,
                PhotoUrl = stagiaire.PhotoUrl,
                Status = stagiaire.Status
            };
        }
    }
}
