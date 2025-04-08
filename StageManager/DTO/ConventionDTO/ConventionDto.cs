using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.ConventionDTO
{
    public class ConventionDto
    {
        public int Id { get; set; }
        public DateTime DateDepot { get; set; }
        public bool EstValidee { get; set; }
        public string CheminFichier { get; set; }
        public int StageId { get; set; }
        public int MembreDirectionId { get; set; }
        public string MembreDirectionNom { get; set; }
    }
}