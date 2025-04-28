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
}