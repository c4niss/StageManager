namespace StageManager.DTO.DomaineDTO
{
    public class DomaineDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        // Informations sur le département
        public int DepartementId { get; set; }
        public string DepartementNom { get; set; }

        // Statistiques
        public int NombreEncadreurs { get; set; }
        public int NombreStages { get; set; }
    }
}