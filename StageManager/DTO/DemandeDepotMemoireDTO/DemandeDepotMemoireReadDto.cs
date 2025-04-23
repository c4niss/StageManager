using StageManager.DTO.MembreDirectionDTO;
using StageManager.Models;
using System;

namespace StageManager.DTOs
{
    public class DemandeDepotMemoireReadDto
    {
        public int Id { get; set; }
        public DateTime DateDemande { get; set; }
        public StatutDepotMemoire Statut { get; set; }

        // Informations sur le thème
        public int ThemeId { get; set; }
        public string NomTheme { get; set; }

        // Informations sur les étudiants
        public string NomPrenomEtudiants { get; set; }

        // Informations sur l'encadreur
        public int EncadreurId { get; set; }
        public string NomPrenomEncadreur { get; set; }
        public string FonctionEncadreur { get; set; }

        // Informations sur le stage
        public int StageId { get; set; }
        public DateTime DateDebutStage { get; set; }
        public DateTime DateFinStage { get; set; }
        public string StagiaireGroup { get; set; }

        // Informations sur la validation
        public bool EstValideParDirection { get; set; }
        public DateTime? DateValidation { get; set; }
        public MinimalMembreDirectionDto MembreDirection { get; set; }

        // Informations sur le mémoire (si disponible)
        public MemoireMinimalDto Memoire { get; set; }
    }
}
