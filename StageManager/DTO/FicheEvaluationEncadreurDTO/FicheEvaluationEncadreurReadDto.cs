using StageManager.Models;

public class FicheEvaluationEncadreurReadDto
{
    public int Id { get; set; }
    public DateTime DateCreation { get; set; }
    public string NomPrenomEncadreur { get; set; }
    public string FonctionEncadreur { get; set; }
    public DateTime DateDebutStage { get; set; }
    public DateTime DateFinStage { get; set; }

    // Catégorie 1: Planification du travail
    public NiveauEvaluationEncadreur FixeObjectifsClairs { get; set; }
    public NiveauEvaluationEncadreur GereImprevus { get; set; }
    public NiveauEvaluationEncadreur RencontreRegulierementEtudiants { get; set; }
    public NiveauEvaluationEncadreur OrganiseEtapesRecherche { get; set; }

    // Catégorie 2: Comprendre et faire comprendre
    public NiveauEvaluationEncadreur ExpliqueClairementContenu { get; set; }
    public NiveauEvaluationEncadreur InterrogeEtudiantsFeedback { get; set; }
    public NiveauEvaluationEncadreur MaitriseConnaissances { get; set; }
    public NiveauEvaluationEncadreur EnseigneFaitDemonstrations { get; set; }

    // Catégorie 3: Susciter la participation
    public NiveauEvaluationEncadreur InviteEtudiantsQuestions { get; set; }
    public NiveauEvaluationEncadreur RepondQuestionsEtudiants { get; set; }
    public NiveauEvaluationEncadreur EncourageInitiativesEtudiants { get; set; }
    public NiveauEvaluationEncadreur InterrogeEtudiantsTravailEffectue { get; set; }
    public NiveauEvaluationEncadreur AccepteExpressionPointsVueDifferents { get; set; }

    // Catégorie 4: Communication orale
    public NiveauEvaluationEncadreur CommuniqueClairementSimplement { get; set; }
    public NiveauEvaluationEncadreur CritiqueConstructive { get; set; }
    public NiveauEvaluationEncadreur PondereQuantiteInformation { get; set; }

    // Catégorie 5: Sens de responsabilité
    public NiveauEvaluationEncadreur EfficaceGestionSupervision { get; set; }
    public NiveauEvaluationEncadreur MaintientAttitudeProfessionnelle { get; set; }
    public NiveauEvaluationEncadreur TransmetDonneesFiables { get; set; }

    // Catégorie 6: Stimuler la motivation des étudiants
    public NiveauEvaluationEncadreur OrienteEtudiantsRessourcesPertinentes { get; set; }
    public NiveauEvaluationEncadreur MontreImportanceSujetTraite { get; set; }
    public NiveauEvaluationEncadreur ProdigueEncouragementsFeedback { get; set; }
    public NiveauEvaluationEncadreur DemontreInteretRecherche { get; set; }

    public string Observations { get; set; }
    public string NomPrenomStagiaireEvaluateur { get; set; }
    public DateTime DateEvaluation { get; set; }

    // Informations liées
    public EncadreurMinimalDto Encadreur { get; set; }
    public StageMinimalDto Stage { get; set; }
}
