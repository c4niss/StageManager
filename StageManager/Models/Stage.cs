﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StageManager.Models
{
    public enum StatutStage { EnAttente , EnCours, Termine, Annule, Prolonge }

    public class Stage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StagiaireGroup { get; set; }

        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public DateTime DateFin { get; set; }

        [Required]
        public StatutStage Statut { get; set; } = StatutStage.EnAttente;

        // Relations modifiées
        [ForeignKey("Convention")]
        public int? ConventionId { get; set; }

        [ForeignKey("Departement")]
        public int? DepartementId { get; set; }

        [ForeignKey("Encadreur")]
        public int? EncadreurId { get; set; }

        // Navigation avec JsonIgnore
        [JsonIgnore]
        public virtual Encadreur Encadreur { get; set; }

        [JsonIgnore]
        public Convention Convention { get; set; }
        public int? DomaineId { get; set; }
        public Domaine Domaine { get; set; }

        [JsonIgnore]
        public Departement Departement { get; set; }

        // Autres propriétés
        public List<Stagiaire> Stagiaires { get; set; }
        public List<FicheEvaluationStagiaire> ficheEvaluationStagiaire { get; set; }
        public List<FicheEvaluationEncadreur> ficheEvaluationEncadreurs { get; set; }
        public int AvenantId { get; set; }
        public int MemoireId { get; set; }
        public Avenant Avenant { get; set; }
        public Memoire Memoire { get; set; }
    }
}