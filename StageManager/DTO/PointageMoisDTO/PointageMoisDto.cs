using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO
{
    public class PointageMoisDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Mois { get; set; }

        [Required]
        public int Annee { get; set; }

        public List<JourPresenceDto> JoursPresence { get; set; }
    }
}
