﻿using StageManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StageManager.DTO.StagiaireDTO
{
    public class StagiaireCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Phone]
        [StringLength(20)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(100)]
        public string Universite { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialite { get; set; }
    }
}