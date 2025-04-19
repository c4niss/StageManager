namespace StageManager.DTO.ThemeDTO
{
    public class ThemeDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int DemandeaccordId { get; set; }
        public int? StageId { get; set; }
        public string StageNom { get; set; }
    }
}