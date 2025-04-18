﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public class Convention
    {
        public enum Statusconvention
        {
            EnCours,
            Accepte,
            Refuse
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateDepot { get; set; } = DateTime.Now;

        [Required]
        public Statusconvention status { get; set; } = Statusconvention.EnCours;

        [Required]
        [StringLength(255)]
        public string CheminFichier { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int? StageId { get; set; }
        [ForeignKey("DemandeAccord")]
        public int DemandeAccordId { get; set; }

        [Required]
        [ForeignKey("MembreDirection")]
        public int MembreDirectionId { get; set; }
        public Demandeaccord DemandeAccord { get; set; }
        public Stage? Stage { get; set; }
        public MembreDirection MembreDirection { get; set; }
    }
}