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
                Status = demande.Status,
                StagiaireId = demande.stagiaires?.Select(s => s.Id).ToList(),
                StagiaireNomComplet = string.Join(", ", demande.stagiaires?
                    .Select(s => $"{s.Nom} {s.Prenom}") ?? Array.Empty<string>()),
                ThemeId = demande.ThemeId,
                ThemeNom = demande.Theme?.Nom,
                EncadreurId = demande.EncadreurId,
                EncadreurNomComplet = demande.Encadreur != null ?
                    $"{demande.Encadreur.Nom} {demande.Encadreur.Prenom}" : null,
                DateDebut = demande.DateDebut,
                DateFin = demande.DateFin,
                NombreSeancesParSemaine = demande.NombreSeancesParSemaine,
                DureeSeances = demande.DureeSeances,
                DateCreation = demande.DateCreation,
                ServiceAccueil = demande.ServiceAccueil,
                UniversiteInstitutEcole = demande.UniversiteInstitutEcole,
                FiliereSpecialite = demande.FiliereSpecialite,
                Telephone = demande.Telephone,
                Email = demande.Email,
                DiplomeObtention = demande.DiplomeObtention,
                NatureStage = demande.NatureStage,
                Nom = demande.Nom,
                Prenom = demande.Prenom,
                DemandeStageId = demande.DemandeStageId
            };
        }
    }
}