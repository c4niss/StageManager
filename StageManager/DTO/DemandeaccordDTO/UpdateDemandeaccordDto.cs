using StageManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.DemandeaccordDTO
{
    public class UpdateDemandeaccordDto
    {
        // Thème (optionnel)
        public int? ThemeId { get; set; }
        // Encadreur (optionnel)
        public int? EncadreurId { get; set; }

        // Période du stage
        [Required(ErrorMessage = "La date de début est requise")]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise")]
        public DateTime DateFin { get; set; }

        // Informations sur les séances
        [Required(ErrorMessage = "Le nombre de séances par semaine est requis")]
        [Range(1, 7, ErrorMessage = "Le nombre de séances doit être entre 1 et 7")]
        public int NombreSeancesParSemaine { get; set; }

        [Required(ErrorMessage = "La durée des séances est requise")]
        [Range(1, 8, ErrorMessage = "La durée des séances doit être entre 1 et 8 heures")]
        public int DureeSeances { get; set; }
    }
}