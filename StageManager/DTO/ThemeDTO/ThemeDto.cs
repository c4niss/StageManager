namespace StageManager.DTO.ThemeDTO
{
    public class ThemeDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int DemandeaccordId { get; set; }
        public int? StageId { get; set; }
        public string StageNom { get; set; }

        public int? DepartementId { get; set; }
        public string DepartementNom { get; set; }

        public int? DomaineId { get; set; }
        public string DomaineNom { get; set; }
    }
}