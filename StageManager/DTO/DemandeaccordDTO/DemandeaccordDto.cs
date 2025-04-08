using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class DemandeaccordDto
    {
        public int Id { get; set; }

        [EnumDataType(typeof(StatusAccord))]
        public StatusAccord Status { get; set; }

        public List<int> StagiaireId { get; set; }
        public string StagiaireNomComplet { get; set; }

        public int ThemeId { get; set; }
        public string ThemeNom { get; set; }

        public int? EncadreurId { get; set; }
        public string EncadreurNomComplet { get; set; }

        // Add missing properties that exist in entity and are used in controllers
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NombreSeancesParSemaine { get; set; }
        public int DureeSeances { get; set; }
        public DateTime DateCreation { get; set; }
    }
}