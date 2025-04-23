using StageManager.Models;
using System;

namespace StageManager.DTOs
{
    public class DemandeDepotMemoireListDto
    {
        public int Id { get; set; }
        public DateTime DateDemande { get; set; }
        public StatutDepotMemoire Statut { get; set; }
        public string NomTheme { get; set; }
        public string NomPrenomEtudiants { get; set; }
        public string NomPrenomEncadreur { get; set; }
        public DateTime DateDebutStage { get; set; }
        public DateTime DateFinStage { get; set; }
        public bool EstValideParDirection { get; set; }
    }
}
