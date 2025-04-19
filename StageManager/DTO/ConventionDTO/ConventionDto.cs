using StageManager.Models;
using System.ComponentModel.DataAnnotations;
using static StageManager.Models.Convention;

namespace StageManager.DTO.ConventionDTO
{
    public class ConventionDto
    {
        public int Id { get; set; }
        public DateTime DateDepot { get; set; }

        public Statusconvention Status { get; set; }
        public string CheminFichier { get; set; }

        // Informations sur le stage associé
        public int? StageId { get; set; }
        public string StageNom { get; set; }

        // Informations sur le membre de direction associé
        public int MembreDirectionId { get; set; }
        public string MembreDirectionNom { get; set; }
    }
}