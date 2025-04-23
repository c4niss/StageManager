using StageManager.DTOs;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class FichePointageMapping
    {
        // Conversion de FichePointage vers FichePointageReadDto
        public static FichePointageReadDto ToReadDto(this FicheDePointage fiche)
        {
            return new FichePointageReadDto
            {
                Id = fiche.Id,
                DateCreation = fiche.DateCreation,
                NomPrenomStagiaire = fiche.NomPrenomStagiaire,
                StructureAccueil = fiche.StructureAccueil,
                NomQualitePersonneChargeSuivi = fiche.NomQualitePersonneChargeSuivi,
                DateDebutStage = fiche.DateDebutStage,
                DateFinStage = fiche.DateFinStage,
                NatureStage = fiche.NatureStage,
                DonneesPointage = fiche.DonneesPointage,
                Stagiaire = fiche.Stagiaire != null ? new StagiaireMinimalDto
                {
                    Id = fiche.Stagiaire.Id,
                    NomPrenom = $"{fiche.Stagiaire.Nom} {fiche.Stagiaire.Prenom}"
                } : null,
                Encadreur = fiche.Encadreur != null ? new EncadreurMinimalDto
                {
                    Id = fiche.Encadreur.Id,
                    NomPrenom = $"{fiche.Encadreur.Nom} {fiche.Encadreur.Prenom}"
                } : null,
                Stage = fiche.Stage != null ? new StageMinimalDto
                {
                    Id = fiche.Stage.Id,
                } : null
            };
        }

        // Conversion de FichePointage vers FichePointageListDto
        public static FichePointageListDto ToListDto(this FicheDePointage fiche)
        {
            return new FichePointageListDto
            {
                Id = fiche.Id,
                NomPrenomStagiaire = fiche.NomPrenomStagiaire,
                StructureAccueil = fiche.StructureAccueil,
                DateDebutStage = fiche.DateDebutStage,
                DateFinStage = fiche.DateFinStage,
                NatureStage = fiche.NatureStage,
                EstComplete = !string.IsNullOrEmpty(fiche.DonneesPointage),
                EstValide = fiche.EstValide
            };
        }

        // Conversion de FichePointageCreateDto vers FichePointage
        public static FicheDePointage ToEntity(this FichePointageCreateDto dto)
        {
            return new FicheDePointage
            {
                DateCreation = DateTime.Now,
                NomPrenomStagiaire = dto.NomPrenomStagiaire,
                StructureAccueil = dto.StructureAccueil,
                NomQualitePersonneChargeSuivi = dto.NomQualitePersonneChargeSuivi,
                DateDebutStage = dto.DateDebutStage,
                DateFinStage = dto.DateFinStage,
                NatureStage = dto.NatureStage,
                StagiaireId = dto.StagiaireId,
                EncadreurId = dto.EncadreurId,
                StageId = dto.StageId,
                EstValide = false
            };
        }

        // Mise à jour de FichePointage à partir de FichePointageUpdateDto
        public static void UpdateFromDto(this FicheDePointage fiche, FichePointageUpdateDto dto)
        {
            fiche.NomPrenomStagiaire = dto.NomPrenomStagiaire;
            fiche.StructureAccueil = dto.StructureAccueil;
            fiche.NomQualitePersonneChargeSuivi = dto.NomQualitePersonneChargeSuivi;
            fiche.DateDebutStage = dto.DateDebutStage;
            fiche.DateFinStage = dto.DateFinStage;
            fiche.NatureStage = dto.NatureStage;

            if (!string.IsNullOrEmpty(dto.DonneesPointage))
            {
                fiche.DonneesPointage = dto.DonneesPointage;
            }
        }
    }
}
