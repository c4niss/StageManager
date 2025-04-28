using System.ComponentModel.DataAnnotations;

namespace StageManager.Models
{
    public class UpdateAdminInfoModel
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Telephone { get; set; }
        public string? PhotoUrl { get; set; }
    }
}