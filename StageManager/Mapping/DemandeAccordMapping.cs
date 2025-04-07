using StageManager.DTO.DemandeaccordDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class DemandeAccordMapping
    {
        public static DemandeaccordDto ToDto(this Demandeaccord demande)
        {
            return new DemandeaccordDto
            {
                Id = demande.Id,
                FichePieceJointe = demande.FichePieceJointe,
                Status = demande.Status,
                StagiaireId = demande.stagiaires?.Select(s => s.Id).ToList(),
                StagiaireNomComplet = string.Join(", ", demande.stagiaires?
                    .Select(s => $"{s.Nom} {s.Prenom}") ?? Array.Empty<string>()),
                ThemeId = demande.ThemeId,
                ThemeNom = demande.Theme?.Nom,
                EncadreurId = demande.EncadreurId,
                EncadreurNomComplet = demande.Encadreur != null ?
                    $"{demande.Encadreur.Nom} {demande.Encadreur.Prenom}" : null
            };
        }

        public static Demandeaccord ToEntity(this CreateDemandeaccordDto dto)
        {
            return new Demandeaccord
            {
                FichePieceJointe = string.Empty, // À remplacer après upload
                Status = StatusAccord.EnAttente,
                ThemeId = dto.ThemeId,
                EncadreurId = dto.EncadreurId,
                DateCreation = DateTime.Now
                // stagiaires sera ajouté dans le contrôleur
            };
        }
    }
}