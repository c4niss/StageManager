namespace StageManager.DTO
{
    public class StatistiquesDto
    {
        public UserTypeStats MembreDirection { get; set; }
        public UserTypeStats Encadreur { get; set; }
        public UserTypeStats ChefDepartement { get; set; }
        public UserTypeStats Stagiaire { get; set; }
        public int TotalUtilisateurs { get; set; }
        public int TotalUtilisateursActifs { get; set; }
    }

    public class UserTypeStats
    {
        public int Total { get; set; }
        public int Actifs { get; set; }
    }

    public class TotalUtilisateursDto
    {
        public int Total { get; set; }
        public int Actifs { get; set; }

    }
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