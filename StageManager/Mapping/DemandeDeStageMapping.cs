using StageManager.DTO.DemandeDeStageDTO;
using StageManager.Models;
using StageManager.Mapping;
using System.Linq;

namespace StageManager.Mapping
{
    public static class DemandeDeStageMapping
    {
        public static DemandeDeStageDto ToDto(this DemandeDeStage demandeDeStage)
        {
            return new DemandeDeStageDto
            {
                Id = demandeDeStage.Id,
                DateDemande = demandeDeStage.DateDemande,
                CheminFichier = demandeDeStage.cheminfichier,
                Statut = demandeDeStage.Statut,
                Commentaire = demandeDeStage.Commentaire,
                Stagiaires = demandeDeStage.Stagiaires?.Select(s => s.ToDto()).ToList() ?? new List<DTO.StagiaireDTO.StagiaireDto>(),
                MembreDirection = demandeDeStage.MembreDirection?.ToDto() ?? new DTO.MembreDirectionDTO.MembreDirectionDto()
            };
        }
    }
}