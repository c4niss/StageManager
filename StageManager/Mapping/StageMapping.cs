using StageManager.DTO.StageDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class StageMapping
    {
        public static StageDto ToDto(this Stage stage)
        {
            return new StageDto
            {
                Id = stage.Id,
                StagiaireGroup = stage.StagiaireGroup,
                DateDebut = stage.DateDebut,
                DateFin = stage.DateFin,
                Statut = stage.Statut,
                ConventionId = stage.ConventionId,
                DepartementId = stage.DepartementId,
                DepartementNom = stage.Departement?.Nom,
                EncadreurId = stage.EncadreurId,
                EncadreurNomComplet = stage.Encadreur?.Nom,
                Stagiaires = stage.Stagiaires?.Select(s => new StagiaireInfoDto
                {
                    Id = s.Id,
                    NomComplet = $"{s.Prenom} {s.Nom}",
                    Email = s.Email
                }).ToList()
            };
        }

        public static Stage ToEntity(this CreateStageDto dto)
        {
            return new Stage
            {
                StagiaireGroup = dto.StagiaireGroup,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                Statut = StatutStage.EnAttente,
                ConventionId = dto.ConventionId,
                DepartementId = dto.DepartementId,
                EncadreurId = dto.EncadreurId
            };
        }
    }
}
