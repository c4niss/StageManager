using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.MembreDirectionDTO
{
    public class UpdateMembreInfoDto
    {
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Format de téléphone invalide")]
        public string? Telephone { get; set; }
    }
}