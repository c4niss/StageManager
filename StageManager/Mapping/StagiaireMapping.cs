﻿using StageManager.DTO.StagiaireDTO;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class StagiaireMapping
    {
        public static StagiaireDto ToDto(this Stagiaire stagiaire)
        {
            return new StagiaireDto
            {
                Id = stagiaire.Id,
                Nom = stagiaire.Nom,
                Prenom = stagiaire.Prenom,
                Email = stagiaire.Email,
                Telephone = stagiaire.Telephone,
                Universite = stagiaire.Universite,
                Specialite = stagiaire.Specialite,
                EstActif = stagiaire.EstActif,
                Status = stagiaire.Status,
                Username = stagiaire.Username,
                StageId = stagiaire.StageId
            };
        }
    }
}