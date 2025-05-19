namespace StageManager.DTO
{
    public class MembreDirectionStatistiquesDto
    {
        // Statistiques des demandes de stage
        public DemandeDeStageStats DemandesDeStage { get; set; }

        // Statistiques des demandes d'accord
        public DemandeAccord1Stats DemandesAccord { get; set; }

        // Statistiques des conventions
        public ConventionStats Conventions { get; set; }

        // Statistiques des stages
        public StageStats Stages { get; set; }

        // Statistiques des attestations
        public AttestationStats Attestations { get; set; }

        // Nombre total de stagiaires
        public int TotalStagiaires { get; set; }
    }

    public class DemandeDeStageStats
    {
        public int Total { get; set; }
        public int EnAttente { get; set; }
        public int Acceptees { get; set; }
        public int Refusees { get; set; }
    }

    public class DemandeAccord1Stats
    {
        public int Total { get; set; }
        public int EnAttente { get; set; }
        public int EnCours { get; set; }
        public int Acceptees { get; set; }
        public int Refusees { get; set; }
    }

    public class ConventionStats
    {
        public int Total { get; set; }
        public int EnAttente { get; set; }
        public int Acceptees { get; set; }
        public int Refusees { get; set; }
    }

    public class AttestationStats
    {
        public int Total { get; set; }
        public int Delivrees { get; set; }
        public int NonDelivrees { get; set; }
    }
}