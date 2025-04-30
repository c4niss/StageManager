using StageManager.DTO.JourPresenceDTO;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.PointageMoisDTO
{
    public class PointageMoisUpdateDto
    {
        [Required]
        public string Mois { get; set; }

        [Required]
        public int Annee { get; set; }

        public List<JourPresenceUpdateDto> JoursPresence { get; set; }
    }
}
