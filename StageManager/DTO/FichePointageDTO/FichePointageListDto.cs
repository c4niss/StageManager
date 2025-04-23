using StageManager.Models;

namespace StageManager.DTOs
{
    public class FichePointageListDto
    {
        public int Id { get; set; }
        public string NomPrenomStagiaire { get; set; }
        public string StructureAccueil { get; set; }
        public DateTime DateDebutStage { get; set; }
        public DateTime DateFinStage { get; set; }
        public NatureStage NatureStage { get; set; }
        public bool EstComplete { get; set; }
        public bool EstValide { get; set; }
    }
}
