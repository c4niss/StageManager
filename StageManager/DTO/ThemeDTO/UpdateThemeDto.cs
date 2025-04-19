using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ThemeDTO
{
    public class UpdateThemeDto
    {
        [StringLength(100)]
        public string Nom { get; set; }
    }
}