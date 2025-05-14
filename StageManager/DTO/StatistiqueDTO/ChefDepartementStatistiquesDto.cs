namespace StageManager.DTO
{
    public class ChefDepartementStatistiquesDto
    {
        public int TotalEncadreurs { get; set; }
        public int EncadreursDisponibles { get; set; }
        public int TotalStagiaires { get; set; }
        public int StagiairesActifs { get; set; }
        public StageStats Stages { get; set; }
        public int TotalDemandesAccord { get; set; }
        public DemandeAccordStats DemandesAccord { get; set; }
        public string NomDepartement { get; set; }
    }

    public class DemandeAccordStats
    {
        public int Total { get; set; }
        public int EnAttente { get; set; }
        public int Approuvees { get; set; }
        public int Rejetees { get; set; }
    }
}