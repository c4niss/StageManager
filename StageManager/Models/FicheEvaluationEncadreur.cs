using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageManager.Models
{
    public enum NiveauEvaluationEncadreur
    {
        Insuffisant = 1,
        Moyen = 2,  
        Bon = 3,        
        Excellent = 4
    }

    public class FicheEvaluationEncadreur
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Identification de l'encadreur
        [Required]
        public string NomPrenomEncadreur { get; set; }

        [Required]
        public string FonctionEncadreur { get; set; }

        // Période du stage
        [Required]
        public DateTime DateDebutStage { get; set; }

        [Required]
        public DateTime DateFinStage { get; set; }

        // 1. Planification du travail
        [Required]
        public NiveauEvaluationEncadreur FixeObjectifsClairs { get; set; }

        [Required]
        public NiveauEvaluationEncadreur GereImprevus { get; set; }

        [Required]
        public NiveauEvaluationEncadreur RencontreRegulierementEtudiants { get; set; }

        [Required]
        public NiveauEvaluationEncadreur OrganiseEtapesRecherche { get; set; }

        // 2. Comprendre et faire comprendre
        [Required]
        public NiveauEvaluationEncadreur ExpliqueClairementContenu { get; set; }

        [Required]
        public NiveauEvaluationEncadreur InterrogeEtudiantsFeedback { get; set; }

        [Required]
        public NiveauEvaluationEncadreur MaitriseConnaissances { get; set; }

        [Required]
        public NiveauEvaluationEncadreur EnseigneFaitDemonstrations { get; set; }

        // 3. Susciter la participation
        [Required]
        public NiveauEvaluationEncadreur InviteEtudiantsQuestions { get; set; }

        [Required]
        public NiveauEvaluationEncadreur RepondQuestionsEtudiants { get; set; }

        [Required]
        public NiveauEvaluationEncadreur EncourageInitiativesEtudiants { get; set; }

        [Required]
        public NiveauEvaluationEncadreur InterrogeEtudiantsTravailEffectue { get; set; }

        [Required]
        public NiveauEvaluationEncadreur AccepteExpressionPointsVueDifferents { get; set; }

        // 4. Communication orale
        [Required]
        public NiveauEvaluationEncadreur CommuniqueClairementSimplement { get; set; }

        [Required]
        public NiveauEvaluationEncadreur CritiqueConstructive { get; set; }

        [Required]
        public NiveauEvaluationEncadreur PondereQuantiteInformation { get; set; }

        // 5. Sens de responsabilité
        [Required]
        public NiveauEvaluationEncadreur EfficaceGestionSupervision { get; set; }

        [Required]
        public NiveauEvaluationEncadreur MaintientAttitudeProfessionnelle { get; set; }

        [Required]
        public NiveauEvaluationEncadreur TransmetDonneesFiables { get; set; }

        // 6. Stimuler la motivation des étudiants
        [Required]
        public NiveauEvaluationEncadreur OrienteEtudiantsRessourcesPertinentes { get; set; }

        [Required]
        public NiveauEvaluationEncadreur MontreImportanceSujetTraite { get; set; }

        [Required]
        public NiveauEvaluationEncadreur ProdigueEncouragementsFeedback { get; set; }

        [Required]
        public NiveauEvaluationEncadreur DemontreInteretRecherche { get; set; }

        // Champ d'observation
        [MaxLength(500)]
        public string Observations { get; set; }

        // Informations sur le stagiaire évaluateur
        [Required]
        public string NomPrenomStagiaireEvaluateur { get; set; }


        // Date d'évaluation
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateEvaluation { get; set; }

        // Relations
        [Required]
        [ForeignKey("Encadreur")]
        public int EncadreurId { get; set; }

        [Required]
        [ForeignKey("Stage")]
        public int StageId { get; set; }
        public virtual Encadreur Encadreur { get; set; }
        public virtual Stage Stage { get; set; }
    }
}