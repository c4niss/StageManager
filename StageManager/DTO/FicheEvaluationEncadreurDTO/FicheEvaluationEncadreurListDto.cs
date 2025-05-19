public class FicheEvaluationEncadreurListDto
{
    public int Id { get; set; }
    public string NomPrenomEncadreur { get; set; }
    public string FonctionEncadreur { get; set; }
    public DateTime DateEvaluation { get; set; }
    public string NomPrenomStagiaireEvaluateur { get; set; }
    public int StagiaireId { get; set; }
    public int StageId { get; set; }
    public bool EstValide { get; set; }

    // Score moyen (optionnel, pour afficher une évaluation globale dans la liste)
    public double ScoreMoyen { get; set; }
}