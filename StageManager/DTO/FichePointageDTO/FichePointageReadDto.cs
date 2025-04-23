using StageManager.Models;

namespace StageManager.DTOs
{
    public class FichePointageReadDto
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
        public string NomPrenomStagiaire { get; set; }
        public string StructureAccueil { get; set; }
        public string NomQualitePersonneChargeSuivi { get; set; }
        public DateTime DateDebutStage { get; set; }
        public DateTime DateFinStage { get; set; }
        public NatureStage NatureStage { get; set; }
        public string DonneesPointage { get; set; }

        // Informations liées
        public StagiaireMinimalDto Stagiaire { get; set; }
        public EncadreurMinimalDto Encadreur { get; set; }
        public StageMinimalDto Stage { get; set; }
    }
}
