using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO
{
    public class JourPresenceDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Jour { get; set; }

        [Required]
        public DayOfWeek JourSemaine { get; set; }

        [Required]
        public bool EstPresent { get; set; }

        public string Commentaire { get; set; }
    }
}
