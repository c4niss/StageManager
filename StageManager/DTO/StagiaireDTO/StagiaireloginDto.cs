using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StagiaireDTO
{
    public class StagiaireloginDto
    {

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string MotDePasse { get; set; }
    }
}
