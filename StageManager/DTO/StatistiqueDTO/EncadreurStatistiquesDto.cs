namespace StageManager.DTO
{
    public class EncadreurStatistiquesDto
    {
        public int TotalStagiaires { get; set; }
        public int StagiairesActifs { get; set; }
        public StageStats Stages { get; set; }
        public int MaxStagiaires { get; set; }
        public bool EstDisponible { get; set; }
    }

    public class StageStats
    {
        public int Total { get; set; }
        public int EnCours { get; set; }
        public int Termines { get; set; }
        public int Annules { get; set; }
        public int EnAttente { get; set; }
        public int Prolonges { get; set; }
    }
}