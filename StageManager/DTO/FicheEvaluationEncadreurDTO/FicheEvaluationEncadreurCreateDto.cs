using StageManager.Models;
using System.ComponentModel.DataAnnotations;

public class FicheEvaluationEncadreurCreateDto
{
    [Required]
    public string NomPrenomEncadreur { get; set; }

    [Required]
    public string FonctionEncadreur { get; set; }

    [Required]
    public DateTime DateDebutStage { get; set; }

    [Required]
    public DateTime DateFinStage { get; set; }

    [Required]
    public NiveauEvaluationEncadreur FixeObjectifsClairs { get; set; }

    [Required]
    public NiveauEvaluationEncadreur GereImprevus { get; set; }

    [Required]
    public NiveauEvaluationEncadreur RencontreRegulierementEtudiants { get; set; }

    [Required]
    public NiveauEvaluationEncadreur OrganiseEtapesRecherche { get; set; }

    [Required]
    public NiveauEvaluationEncadreur ExpliqueClairementContenu { get; set; }

    [Required]
    public NiveauEvaluationEncadreur InterrogeEtudiantsFeedback { get; set; }

    [Required]
    public NiveauEvaluationEncadreur MaitriseConnaissances { get; set; }

    [Required]
    public NiveauEvaluationEncadreur EnseigneFaitDemonstrations { get; set; }

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

    [Required]
    public NiveauEvaluationEncadreur CommuniqueClairementSimplement { get; set; }

    [Required]
    public NiveauEvaluationEncadreur CritiqueConstructive { get; set; }

    [Required]
    public NiveauEvaluationEncadreur PondereQuantiteInformation { get; set; }

    [Required]
    public NiveauEvaluationEncadreur EfficaceGestionSupervision { get; set; }

    [Required]
    public NiveauEvaluationEncadreur MaintientAttitudeProfessionnelle { get; set; }

    [Required]
    public NiveauEvaluationEncadreur TransmetDonneesFiables { get; set; }

    [Required]
    public NiveauEvaluationEncadreur OrienteEtudiantsRessourcesPertinentes { get; set; }

    [Required]
    public NiveauEvaluationEncadreur MontreImportanceSujetTraite { get; set; }

    [Required]
    public NiveauEvaluationEncadreur ProdigueEncouragementsFeedback { get; set; }

    [Required]
    public NiveauEvaluationEncadreur DemontreInteretRecherche { get; set; }

    [MaxLength(500)]
    public string Observations { get; set; }

    [Required]
    public string NomPrenomStagiaireEvaluateur { get; set; }
    [Required]
    public string StagiaireId { get; set; }
    [Required]
    public int EncadreurId { get; set; }

    [Required]
    public int StageId { get; set; }
}
